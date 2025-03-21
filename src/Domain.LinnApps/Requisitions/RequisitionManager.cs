namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Domain;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Stores;

    public class RequisitionManager : IRequisitionManager
    {
        private readonly IAuthorisationService authService;
        
        private readonly IRepository<RequisitionHeader, int> repository;

        private readonly IRequisitionStoredProcedures requisitionStoredProcedures;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<Part, string> partRepository;

        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        private readonly IRepository<StoresTransactionDefinition, string> transactionDefinitionRepository;

        private readonly ITransactionManager transactionManager;

        private readonly IStoresService storesService;

        private readonly IRepository<StoresPallet, int> palletRepository;

        private readonly IRepository<StockState, string> stateRepository;

        private readonly IRepository<StockPool, string> stockPoolRepository;

        private readonly IRepository<StoresFunction, string> storesFunctionRepository;

        private readonly IRepository<Department, string> departmentRepository;

        private readonly IRepository<Nominal, string> nominalRepository;

        public RequisitionManager(
            IAuthorisationService authService,
            IRepository<RequisitionHeader, int> repository,
            IRequisitionStoredProcedures requisitionStoredProcedures,
            IRepository<Employee, int> employeeRepository,
            IRepository<Part, string> partRepository,
            IRepository<StorageLocation, int> storageLocationRepository,
            IRepository<StoresTransactionDefinition, string> transactionDefinitionRepository,
            ITransactionManager transactionManager,
            IStoresService storesService,
            IRepository<StoresPallet, int> palletRepository,
            IRepository<StockState, string> stateRepository,
            IRepository<StockPool, string> stockPoolRepository,
            IRepository<StoresFunction, string> storesFunctionRepository,
            IRepository<Department, string> departmentRepository,
            IRepository<Nominal, string> nominalRepository)
        {
            this.authService = authService;
            this.repository = repository;
            this.requisitionStoredProcedures = requisitionStoredProcedures;
            this.employeeRepository = employeeRepository;
            this.partRepository = partRepository;
            this.storageLocationRepository = storageLocationRepository;
            this.transactionDefinitionRepository = transactionDefinitionRepository;
            this.transactionManager = transactionManager;
            this.storesService = storesService;
            this.palletRepository = palletRepository;
            this.stateRepository = stateRepository;
            this.stockPoolRepository = stockPoolRepository;
            this.storesFunctionRepository = storesFunctionRepository;
            this.departmentRepository = departmentRepository;
            this.nominalRepository = nominalRepository;
        }
        
        public async Task<RequisitionHeader> CancelHeader(
            int reqNumber,
            int cancelledBy,
            IEnumerable<string> privileges,
            string reason,
            bool requiresAuth = true)
        {
            if (requiresAuth && !this.authService.HasPermissionFor(
                    AuthorisedActions.CancelRequisition, privileges))
            {
                throw new UnauthorisedActionException(
                    "You do not have permission to cancel a requisition");
            }

            var req = await this.repository.FindByIdAsync(reqNumber);
            
            if (string.IsNullOrEmpty(req.StoresFunction.CancelFunction))
            {
                var by = await this.employeeRepository.FindByIdAsync(cancelledBy);
                req.Cancel(reason, by);
            }
            else if (req.StoresFunction.CancelFunction == "UNALLOC_REQ")
            {
                var unallocateReqResult = await this.requisitionStoredProcedures.UnallocateRequisition(
                    reqNumber, null, cancelledBy);

                if (!unallocateReqResult.Success)
                {
                    throw new CancelRequisitionException(unallocateReqResult.Message);
                }
            }
            else
            {
                throw new CancelRequisitionException(
                    "Cannot cancel req - invalid cancel function");
            }

            var deleteAllocsOntoResult = await this.requisitionStoredProcedures.DeleteAllocOntos(
                                             reqNumber,
                                             null,
                                             req.Document1,
                                             req.Document1Name);

            if (!deleteAllocsOntoResult.Success)
            {
                throw new CancelRequisitionException(deleteAllocsOntoResult.Message);
            }

            return req;
        }

        public async Task<RequisitionHeader> CancelLine(
            int reqNumber, 
            int lineNumber,
            int cancelledBy,
            IEnumerable<string> privileges,
            string reason)
        {
            if (!this.authService.HasPermissionFor(
                    AuthorisedActions.CancelRequisition, privileges))
            {
                throw new UnauthorisedActionException(
                    "You do not have permission to cancel a requisition");
            }

            var req = await this.repository.FindByIdAsync(reqNumber);

            if (string.IsNullOrEmpty(req.StoresFunction.CancelFunction))
            {
                var by = await this.employeeRepository.FindByIdAsync(cancelledBy);
                req.CancelLine(lineNumber, reason, by);
            }
            else if (req.StoresFunction.CancelFunction == "UNALLOC_REQ")
            {
                var unallocateReqResult = await this.requisitionStoredProcedures.UnallocateRequisition(
                                              reqNumber, lineNumber, cancelledBy);

                if (!unallocateReqResult.Success)
                {
                    throw new RequisitionException(unallocateReqResult.Message);
                }
            }
            else
            {
                throw new RequisitionException(
                    "Cannot cancel req - invalid cancel function");
            }

            var deleteAllocsOntoResult = await this.requisitionStoredProcedures.DeleteAllocOntos(
                                             reqNumber,
                                             lineNumber,
                                             req.Document1,
                                             req.Document1Name);

            if (!deleteAllocsOntoResult.Success)
            {
                throw new RequisitionException(deleteAllocsOntoResult.Message);
            }

            return req;
        }

        public async Task<RequisitionHeader> BookRequisition(
            int reqNumber, 
            int? lineNumber,
            int bookedBy,
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(
                    AuthorisedActions.BookRequisition, privileges))
            {
                throw new UnauthorisedActionException(
                    "You do not have permission to book a requisition");
            }

            var doRequisitionResult = await this.requisitionStoredProcedures.DoRequisition(
                reqNumber,
                lineNumber,
                bookedBy);

            if (!doRequisitionResult.Success)
            {
                throw new RequisitionException(doRequisitionResult.Message);
            }

            var req = await this.repository.FindByIdAsync(reqNumber);
            return req;
        }

        public async Task<RequisitionHeader> AuthoriseRequisition(
            int reqNumber, 
            int authorisedBy,
            IEnumerable<string> privileges)
        {
            var req = await this.repository.FindByIdAsync(reqNumber);

            if (req == null)
            {
                throw new RequisitionException("Req not found");
            }

            if (!this.authService.HasPermissionFor(
                    req.AuthorisePrivilege(), privileges))
            {
                throw new UnauthorisedActionException(
                    "You do not have permission to authorise this requisition");
            }

            var by = await this.employeeRepository.FindByIdAsync(authorisedBy);

            if (by == null)
            {
                throw new RequisitionException("Authorised by not found");
            }

            req.Authorise(by);

            return req;
        }

        public async Task AddMovesToLine(RequisitionLine line, IEnumerable<MoveSpecification> moves)
        {
            var moveSpecifications = moves.ToList();
            await this.CheckMoves(line.Part.PartNumber, moveSpecifications);
            
            // only implementing moves onto for now
            foreach (var moveOnto
                     in moveSpecifications.Where(x => x.ToPallet.HasValue || !string.IsNullOrEmpty(x.ToLocation)))
            {
                var insertOntosResult = await this.requisitionStoredProcedures.InsertReqOntos(
                    line.ReqNumber,
                    moveOnto.Qty,
                    line.LineNumber,
                    moveOnto.ToLocationId,
                    moveOnto.ToPallet,
                    moveOnto.ToStockPool,
                    moveOnto.ToState,
                    "FREE");

                if (!insertOntosResult.Success)
                {
                    throw new InsertReqOntosException($"Failed in insert_req_ontos: {insertOntosResult.Message}");
                }
            }
        }

        public async Task CheckMoves(string partNumber, IEnumerable<MoveSpecification> moves)
        {
            foreach (var moveSpecification in moves)
            {
                // just checking moves onto for now, but could extend if required
                if (moveSpecification.ToPallet.HasValue || !string.IsNullOrEmpty(moveSpecification.ToLocation))
                {
                    if (moveSpecification.ToPallet.HasValue)
                    {
                        // todo - check pallet exists?
                        var canPutPartOnPallet = await this.requisitionStoredProcedures.CanPutPartOnPallet(partNumber,
                            moveSpecification.ToPallet.Value);
                        if (!canPutPartOnPallet)
                        {
                            throw new CannotPutPartOnPalletException(
                                $"Cannot put part {partNumber} onto P{moveSpecification.ToPallet}");
                        }
                    }

                    if (!string.IsNullOrEmpty(moveSpecification.ToLocation))
                    {
                        var toLocation = await this.storageLocationRepository.FindByAsync(x => x.LocationCode == moveSpecification.ToLocation);
                        if (toLocation == null)
                        {
                            throw new InsertReqOntosException($"Location {moveSpecification.ToLocation} not found");
                        }
                        
                        moveSpecification.ToLocationId = toLocation.LocationId;
                    }
                }
            }
        }

        public async Task AddRequisitionLine(RequisitionHeader header, LineCandidate toAdd)
        {
            var part = await this.partRepository.FindByIdAsync(toAdd.PartNumber);
            var transactionDefinition = await this.transactionDefinitionRepository.FindByIdAsync(toAdd.TransactionDefinition);

            header.AddLine(new RequisitionLine(header.ReqNumber, toAdd.LineNumber, part, toAdd.Qty, transactionDefinition));

            // we need this so the line exists for the stored procedure calls coming next
            await this.transactionManager.CommitAsync();

            var createdMoves = false;
            
            if (toAdd.Moves != null && toAdd.Moves.Any())
            {
                await this.CheckMoves(toAdd.PartNumber, toAdd.Moves);

                // for now, assuming moves are either a write on or off, i.e. not a move from one place to another
                // write offs
                foreach (var pick in toAdd.Moves.Where(x => x.FromPallet.HasValue 
                                                            || !string.IsNullOrEmpty(x.FromLocation)))
                {
                    var fromLocation = string.IsNullOrEmpty(pick.FromLocation)
                                           ? null
                                           : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == pick.FromLocation);
                    var pickResult = await this.requisitionStoredProcedures.PickStock(
                                         toAdd.PartNumber,
                                         header.ReqNumber,
                                         toAdd.LineNumber,
                                         pick.Qty,
                                         fromLocation?.LocationId,
                                         pick.FromPallet,
                                         pick.FromStockPool,
                                         toAdd.TransactionDefinition);

                    if (!pickResult.Success)
                    {
                        throw new PickStockException("failed in pick_stock: " + pickResult.Message);
                    }

                    createdMoves = true;
                }

                // write ons
                foreach (var moveOnto 
                         in toAdd.Moves.Where(x => x.ToPallet.HasValue || !string.IsNullOrEmpty(x.ToLocation)))
                {
                    var insertOntosResult = await this.requisitionStoredProcedures.InsertReqOntos(
                                                header.ReqNumber,
                                                moveOnto.Qty,
                                                toAdd.LineNumber,
                                                moveOnto.ToLocationId,
                                                moveOnto.ToPallet,
                                                moveOnto.ToStockPool,
                                                moveOnto.ToState,
                                                "FREE", // TODO
                                                createdMoves ? "U" : "I");

                    if (!insertOntosResult.Success)
                    {
                        throw new InsertReqOntosException($"Failed in insert_req_ontos: {insertOntosResult.Message}");
                    }
                }
            }

            // todo - consider what has to happen if a move is from one place to another
            // i.e. cases where both a 'from' and a 'to' location or pallet is set
            var createPostingsResult = await this.requisitionStoredProcedures.CreateNominals(
                                           header.ReqNumber,
                                           toAdd.Qty,
                                           toAdd.LineNumber,
                                           header.Nominal?.NominalCode,
                                           header.Department?.DepartmentCode);

            if (!createPostingsResult.Success)
            {
                throw new CreateNominalPostingException("failed in create_nominals: " + createPostingsResult.Message);
            }
        }

        public async Task CheckAndBookRequisitionHeader(RequisitionHeader header)
        {
            await this.DoChecksForHeaderWithPartSpecified(header);

            await this.repository.AddAsync(header);

            DoProcessResultCheck(  
                await this.requisitionStoredProcedures.CreateRequisitionLines(header.ReqNumber, null));

            DoProcessResultCheck(await this.requisitionStoredProcedures.CanBookRequisition(
                                     header.ReqNumber,
                                     null,
                                     header.Quantity.GetValueOrDefault()));

            DoProcessResultCheck(await this.requisitionStoredProcedures.DoRequisition(
                                     header.ReqNumber,
                                     null,
                                     header.CreatedBy.Id));
        }

        public async Task<RequisitionHeader> CreateLoanReq(int loanNumber)
        {
            var proxyResult = await this.requisitionStoredProcedures.CreateLoanReq(loanNumber);

            if (!proxyResult.Success)
            {
                throw new CreateRequisitionException(proxyResult.Message);
            }

            var reqNumber = Convert.ToInt32(proxyResult.Message);

            return await this.repository.FindByIdAsync(reqNumber);
        }

        public async Task<RequisitionHeader> PickStockOnRequisitionLine(RequisitionHeader header, LineCandidate lineWithPicks)
        {
            var line = header.Lines.SingleOrDefault(l => l.LineNumber == lineWithPicks.LineNumber);

            if (line == null)
            {
                throw new PickStockException("Could not find line");
            }

            // if no moves before
            if (!line.Moves.Any())
            {
                foreach (var pick in lineWithPicks.Moves.Where(x => x.FromPallet.HasValue || !string.IsNullOrEmpty(x.FromLocation)))
                {
                    var fromLocation = string.IsNullOrEmpty(pick.FromLocation)
                        ? null
                        : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == pick.FromLocation);

                    // warning this call only makes the From side of the req_moves you still need to fill out other side
                    var pickResult = await this.requisitionStoredProcedures.PickStock(
                            lineWithPicks.PartNumber,
                            header.ReqNumber,
                            lineWithPicks.LineNumber,
                            pick.Qty,
                            fromLocation?.LocationId, // todo - do we pass a value here if palletNumber?
                            pick.FromPallet,
                            header.FromStockPool,
                            lineWithPicks.TransactionDefinition);

                    if (!pickResult.Success)
                    {
                        throw new PickStockException("failed in pick_stock: " + pickResult.Message);
                    }

                    // now fetch the header with the moves
                    var pickedRequisition = await this.repository.FindByIdAsync(header.ReqNumber);

                    // now if our transaction is an onto transaction need to fix moves
                    var transaction =
                            await this.transactionDefinitionRepository.FindByIdAsync(
                                lineWithPicks.TransactionDefinition);

                    if (transaction.RequiresOntoTransactions)
                    {
                        var pickedLine = pickedRequisition.Lines.SingleOrDefault(l => l.LineNumber == lineWithPicks.LineNumber);
                        if (pickedLine != null)
                        {
                            foreach (var move in pickedLine.Moves)
                            {
                                move.SetOntoFieldsFromHeader(header);
                            }

                            // we need these saved now in case we are picking multiple req lines and lose the changes
                            await this.transactionManager.CommitAsync();
                        }
                    }

                    return pickedRequisition;
                }
            }

            return header;
        }

        public async Task UpdateRequisition(
            RequisitionHeader current, 
            string updatedComments,
            IEnumerable<LineCandidate> lineUpdates)
        {
            // todo - permission checks? will be different for different req types I assume
            current.Update(updatedComments);

            foreach (var line in lineUpdates)
            {
                var existingLine = current.Lines.SingleOrDefault(l => l.LineNumber == line.LineNumber);
                
                // are we updating an existing line?
                if (existingLine != null)
                {
                    // picking stock for an existing line
                    if (line.StockPicked.GetValueOrDefault())
                    {
                        await this.PickStockOnRequisitionLine(current, line);
                    }
                    
                    // adding moves for an existing line
                    var movesToAdd = line.Moves.Where(x => x.IsAddition).ToList();

                    if (movesToAdd.Count <= 0)
                    {
                        continue;
                    }

                    await this.CheckMoves(line.PartNumber, movesToAdd);
                    await this.AddMovesToLine(existingLine, movesToAdd);
                }
                else
                {
                    // adding a new line
                    // note - this will pick stock/create ontos, create nominal postings etc
                    // might need to rethink if not all new lines need this behaviour (update strategies? some other pattern)
                    await this.CheckMoves(line.PartNumber, line.Moves);
                    await this.AddRequisitionLine(current, line);
                }
            }
        }

        public async Task<RequisitionHeader> Validate(
            int createdBy,
            string functionCode,
            string reqType,
            int? document1Number,
            string document1Type,
            string departmentCode,
            string nominalCode,
            LineCandidate firstLine = null,
            string reference = null,
            string comments = null,
            string manualPick = null,
            string fromStockPool = null,
            string toStockPool = null,
            int? fromPalletNumber = null,
            int? toPalletNumber = null,
            string fromLocationCode = null,
            string toLocationCode = null,
            string partNumber = null,
            decimal? quantity = null,
            string fromState = null,
            string toState = null,
            string batchRef = null,
            DateTime? batchDate = null,
            int? document1Line = null)
        {
            // just try and construct a req with a single line
            // exceptions will be thrown if any of the validation fails
            var function = !string.IsNullOrEmpty(functionCode) 
                               ? await this.storesFunctionRepository.FindByIdAsync(functionCode) : null;
            var dept = !string.IsNullOrEmpty(departmentCode) 
                           ? await this.departmentRepository.FindByIdAsync(departmentCode) : null;
            var nom = !string.IsNullOrEmpty(nominalCode)
                          ? await this.nominalRepository.FindByIdAsync(nominalCode) : null;
            var fromLocation = !string.IsNullOrEmpty(fromLocationCode) 
                                   ? await this.storageLocationRepository.FindByAsync(x => x.LocationCode == fromLocationCode) : null;
            var toLocation = !string.IsNullOrEmpty(toLocationCode)
                                 ? await this.storageLocationRepository.FindByAsync(x => x.LocationCode == toLocationCode) : null;
            var part = !string.IsNullOrEmpty(partNumber) ? await this.partRepository.FindByIdAsync(partNumber) : null;

            var employee = await this.employeeRepository.FindByIdAsync(createdBy);

            var req = new RequisitionHeader(
                employee,
                function,
                reqType,
                document1Number,
                document1Type,
                dept,
                nom,
                reference,
                comments,
                manualPick,
                fromStockPool,
                toStockPool,
                fromPalletNumber,
                toPalletNumber,
                fromLocation,
                toLocation,
                part,
                quantity,
                document1Line,
                toState,
                fromState,
                batchRef,
                batchDate);

            // loan out is weird in that all the creation actually happens in PLSQL
            // so don't validate anything else here
            // TODO - are there any other cases where we want to skip further validation?
            // TODO - if so, is there a better way to group these cases than checking function code?
            if (functionCode == "LOAN OUT")
            {
                // could make some proxy calls to check a valid loan number was entered
                return req;
            }
            
            if (firstLine != null)
            {
                var firstLinePart = !string.IsNullOrEmpty(firstLine?.PartNumber)
                                        ? await this.partRepository.FindByIdAsync(firstLine.PartNumber)
                                        : null;
                var transactionDefinition = !string.IsNullOrEmpty(firstLine?.TransactionDefinition)
                                                ? await this.transactionDefinitionRepository.FindByIdAsync(firstLine.TransactionDefinition) : null;

                req.AddLine(new RequisitionLine(
                    0,
                    1,
                    firstLinePart,
                    firstLine.Qty,
                    transactionDefinition,
                    firstLine.Document1,
                    firstLine.Document1Line.GetValueOrDefault(),
                    firstLine.Document1Type));
            }
            
            if (req.Part == null && req.Lines.Count == 0)
            {
                throw new RequisitionException("Lines are required if header does not specify part");
            }

            if (req.Part != null && req.Lines.Count == 0)
            {
                if (FieldIsNeededOrOptional(req.StoresFunction.QuantityRequired) && req.Quantity == null)
                {
                    throw new RequisitionException("A quantity must be entered");
                }

                if (FieldIsNeededOrOptional(req.StoresFunction.FromLocationRequired) && req.FromLocation == null && req.FromPalletNumber == null)
                {
                    throw new RequisitionException("A from location or pallet is required");
                }

                if (FieldIsNeededOrOptional(req.StoresFunction.FromStockPoolRequired) && string.IsNullOrEmpty(req.FromStockPool))
                {
                    throw new RequisitionException("A from stock pool is required");
                }

                if (FieldIsNeededOrOptional(req.StoresFunction.FromStateRequired) && string.IsNullOrEmpty(req.FromState))
                {
                    throw new RequisitionException("A from state is required");
                }

                if (FieldIsNeededOrOptional(req.StoresFunction.ToLocationRequired) && req.ToLocation == null && req.ToPalletNumber == null)
                {
                    throw new RequisitionException("A to location or pallet is required");
                }

                if (FieldIsNeededOrOptional(req.StoresFunction.ToStockPoolRequired) && string.IsNullOrEmpty(req.ToStockPool))
                {
                    throw new RequisitionException("A to stock pool is required");
                }

                if (FieldIsNeededOrOptional(req.StoresFunction.ToStateRequired) && string.IsNullOrEmpty(req.ToState))
                {
                    throw new RequisitionException("A to state is required");
                }

                await this.DoChecksForHeaderWithPartSpecified(req);
            }

            return req;
        }

        private static void DoProcessResultCheck(ProcessResult result)
        {
            if (!result.Success)
            {
                throw new RequisitionException(result.Message);
            }
        }

        private static bool FieldIsNeededOrOptional(string code)
        {
            if (code == "Y" || code == "O")
            {
                return true;
            }

            return false;
        }

        private async Task DoChecksForHeaderWithPartSpecified(RequisitionHeader header)
        {
            StoresPallet toPallet = null;
            if (header.ToPalletNumber.HasValue)
            {
                toPallet = await this.palletRepository.FindByIdAsync(header.ToPalletNumber.Value);
                if (toPallet == null)
                {
                    throw new RequisitionException($"Pallet number {header.ToPalletNumber} does not exist");
                }
            }

            var toState = await this.stateRepository.FindByIdAsync(header.ToState);
            if (!string.IsNullOrEmpty(header.ToState) && toState == null)
            {
                throw new RequisitionException($"To State {header.ToState} does not exist");
            }

            DoProcessResultCheck(await this.storesService.ValidOntoLocation(
                                     header.Part,
                                     header.ToLocation,
                                     toPallet,
                                     toState));

            DoProcessResultCheck(await this.storesService.ValidState(
                                     null,
                                     header.StoresFunction,
                                     header.FromState,
                                     "F"));

            DoProcessResultCheck(await this.storesService.ValidState(
                                     null,
                                     header.StoresFunction,
                                     header.ToState,
                                     "O"));

            var stockPool = await this.stockPoolRepository.FindByIdAsync(header.ToStockPool);
            if (!string.IsNullOrEmpty(header.ToStockPool) && stockPool == null)
            {
                throw new RequisitionException($"To Stock Pool {header.ToStockPool} does not exist");
            }

            DoProcessResultCheck(this.storesService.ValidStockPool(header.Part, stockPool));
        }
    }
}
