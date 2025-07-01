namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Domain;
    using Linn.Common.Domain.Exceptions;
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
        
        private readonly IStockService stockService;

        private readonly ISalesProxy salesProxy;

        private readonly IRepository<PotentialMoveDetail, PotentialMoveDetailKey> potentialMoveRepository;

        private readonly IBomVerificationProxy bomVerificationProxy;

        private readonly IRepository<BookInOrderDetail, BookInOrderDetailKey> bookInOrderDetailRepository;

        private readonly IQueryRepository<AuditLocation> auditLocationRepository;

        private readonly IRepository<StoresPallet, int> palletRepository;

        private readonly IRepository<StockState, string> stateRepository;

        private readonly IRepository<StockPool, string> stockPoolRepository;

        private readonly IRepository<StoresFunction, string> storesFunctionRepository;

        private readonly IRepository<Department, string> departmentRepository;

        private readonly IRepository<Nominal, string> nominalRepository;

        private readonly IDocumentProxy documentProxy;

        private readonly ISerialNumberService serialNumberService;

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
            IRepository<Nominal, string> nominalRepository,
            IDocumentProxy documentProxy,
            IStockService stockService,
            ISalesProxy salesProxy,
            IRepository<PotentialMoveDetail, PotentialMoveDetailKey> potentialMoveRepository,
            IBomVerificationProxy bomVerificationProxy,
            IRepository<BookInOrderDetail, BookInOrderDetailKey> bookInOrderDetailRepository,
            ISerialNumberService serialNumberService,
            IQueryRepository<AuditLocation> auditLocationRepository)
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
            this.documentProxy = documentProxy;
            this.stockService = stockService;
            this.salesProxy = salesProxy;
            this.potentialMoveRepository = potentialMoveRepository;
            this.bomVerificationProxy = bomVerificationProxy;
            this.bookInOrderDetailRepository = bookInOrderDetailRepository;
            this.serialNumberService = serialNumberService;
            this.auditLocationRepository = auditLocationRepository;
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

        public async Task<RequisitionHeader> UnpickRequisitionMove(int reqNumber, int lineNumber, int seq, decimal qtyToUnpick, int unpickedBy, bool reallocate,
            IEnumerable<string> privileges)
        {
            if (qtyToUnpick <= 0)
            {
                throw new RequisitionException($"Must unpick positive qty not {qtyToUnpick}");
            }

            var req = await this.repository.FindByIdAsync(reqNumber);

            if (req == null)
            {
                throw new RequisitionException($"Req {reqNumber} not found");
            }

            var line = req.Lines.SingleOrDefault(l => l.LineNumber == lineNumber);
            if (line == null)
            {
                throw new RequisitionException($"Line {lineNumber} not found on req {reqNumber}");
            }

            if (line.IsBooked())
            {
                throw new RequisitionException($"Line {lineNumber} on req {reqNumber} is already booked");
            }

            if (line.IsCancelled())
            {
                throw new RequisitionException($"Line {lineNumber} on req {reqNumber} is already cancelled");
            }

            var move = line.Moves.SingleOrDefault(mv => mv.Sequence == seq);
            if (move == null)
            {
                throw new RequisitionException($"Line {lineNumber} on Req {reqNumber} has no move with seq {seq}");
            }

            if (!move.CanUnPick())
            {
                throw new RequisitionException($"Move {seq} of Line {lineNumber} on Req {reqNumber} cannot be unpicked");
            }

            if (qtyToUnpick > move.Quantity)
            {
                throw new RequisitionException($"Cannot unpick more than the qty allocated on the move which is {move.Quantity}");
            }

            var result = await this.requisitionStoredProcedures.UnPickStock(
                move.ReqNumber, 
                move.LineNumber, 
                move.Sequence,
                line.Document1Number, 
                line.Document1Line, 
                qtyToUnpick, 
                move.StockLocator?.Id, 
                unpickedBy, 
                reallocate,
                req.StoresFunction.UpdateSalesOrderDetQtyOutstanding == "Y");

            if (!result.Success)
            {
                throw new RequisitionException($"Failed to unpick stock: {result.Message}");
            }

            var unpickedReq = await this.repository.FindByIdAsync(reqNumber);
            return unpickedReq;
        }

        public async Task AddRequisitionLine(RequisitionHeader header, LineCandidate toAdd)
        {
            var part = await this.partRepository.FindByIdAsync(toAdd.PartNumber);
            var transactionDefinition = await this.transactionDefinitionRepository.FindByIdAsync(toAdd.TransactionDefinition);
            var line = new RequisitionLine(header.ReqNumber, toAdd.LineNumber, part, toAdd.Qty, transactionDefinition);

            if (toAdd.SerialNumbers != null)
            {
                foreach (var serialNumber in toAdd.SerialNumbers)
                {
                    line.AddSerialNumber(serialNumber);
                }
            }

            header.AddLine(line);

            // we need this so the line exists for the stored procedure calls coming next
            await this.transactionManager.CommitAsync();

            var createdMoves = false;
            var movesToAdd = toAdd.Moves?.ToList();

            if (movesToAdd != null && movesToAdd.Any())
            {
                var toLocationRequired = header.ReqType != "F" && header.StoresFunction.ToLocationRequiredOrOptional()
                                                               && header.StoresFunction.FunctionCode != "AUDIT";
                await this.CheckMoves(toAdd.PartNumber, movesToAdd, toLocationRequired);

                // for now, assuming moves are either a write on or off, i.e. not a move from one place to another
                // write offs
                foreach (var pick in movesToAdd.Where(x => x.FromPallet.HasValue 
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
                         in movesToAdd.Where(x => x.ToPallet.HasValue || !string.IsNullOrEmpty(x.ToLocation)))
                {
                    var insertOntosResult = await this.requisitionStoredProcedures.InsertReqOntos(
                                                header.ReqNumber,
                                                moveOnto.Qty,
                                                toAdd.LineNumber,
                                                moveOnto.ToLocationId,
                                                moveOnto.ToPallet,
                                                moveOnto.ToStockPool,
                                                moveOnto.ToState,
                                                "FREE",
                                                createdMoves ? "U" : "I");

                    if (!insertOntosResult.Success)
                    {
                        throw new InsertReqOntosException($"Failed in insert_req_ontos: {insertOntosResult.Message}");
                    }
                }
            }
            else
            {
                var insertOrUpdateMoves = "I";
                if (header.ManualPick == "N" && transactionDefinition.RequiresStockAllocations)
                {
                    // todo can we make automatic from picks see CREATE_REQ_MOVES in REQ_UT.fmb
                    var autopickResult = await this.requisitionStoredProcedures.PickStock(
                        toAdd.PartNumber,
                        header.ReqNumber,
                        toAdd.LineNumber,
                        toAdd.Qty,
                        header.FromLocation?.LocationId,
                        header.FromPalletNumber,
                        header.FromStockPool,
                        toAdd.TransactionDefinition);
                    
                    insertOrUpdateMoves = "U";

                    if (!autopickResult.Success)
                    {
                        throw new PickStockException("failed in pick_stock: " + autopickResult.Message);
                    }
                }
                
                // todo what about the ontos
                if (transactionDefinition.RequiresOntoTransactions)
                {
                    var ontoResult = await this.requisitionStoredProcedures.InsertReqOntos(
                        header.ReqNumber,
                        toAdd.Qty,
                        toAdd.LineNumber,
                        header.ToLocation?.LocationId,
                        header.ToPalletNumber,
                        header.ToStockPool,
                        header.ToState,
                        "FREE",
                        insertOrUpdateMoves);

                    if (!ontoResult.Success)
                    {
                        throw new InsertReqOntosException($"Failed in insert_req_ontos: {ontoResult.Message}");
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

        public async Task CreateLinesAndBookAutoRequisitionHeader(RequisitionHeader header)
        {
            try
            {
                DoProcessResultCheck(
                    await this.requisitionStoredProcedures.CreateRequisitionLines(header.ReqNumber, null));

                // from REQ_UT REQHEADS.POST-INSERT
                // function codes that are function_Type A and process stage 1 incl LOAN OUT, SUKIT
                if (header.StoresFunction.ProcessStage == 2)
                {
                    await this.CheckAndBookRequisition(header);
                }
            }
            catch (DomainException e)
            {
                // cancel the req so it doesn't hang around unbooked and mess up validation
                var savedReq = await this.repository.FindByIdAsync(header.ReqNumber);
                savedReq.Cancel(e.Message, header.CreatedBy);
                if (savedReq.Lines?.Count > 0)
                {
                    foreach (var requisitionLine in savedReq.Lines)
                    {
                        requisitionLine.Cancel(header.CreatedBy.Id, e.Message, DateTime.Now);
                    }
                }

                await this.transactionManager.CommitAsync();

                throw;
            }
        }

        public async Task CheckAndBookRequisition(RequisitionHeader header)
        {
            DoProcessResultCheck(header.RequisitionCanBeBooked());

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
                            fromLocation?.LocationId,
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
                        var pickedLine = pickedRequisition.Lines
                            .SingleOrDefault(l => l.LineNumber == lineWithPicks.LineNumber);
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
            string updatedReference,
            IEnumerable<LineCandidate> lineUpdates)
        {
            // todo - permission checks? will be different for different req types I assume
            current.Update(updatedComments, updatedReference);

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

                    await this.CheckMoves(
                        line.PartNumber,
                        movesToAdd,
                        current.ReqType != "F" && current.StoresFunction.ToLocationRequiredOrOptional());
                    await this.AddMovesToLine(existingLine, movesToAdd);
                }
                else
                {
                    // adding a new line
                    // note - this will pick stock/create ontos, create nominal postings etc
                    // might need to rethink if not all new lines need this behaviour (update strategies? some other pattern)
                    await this.CheckMoves(
                        line.PartNumber,
                        line.Moves == null ? new List<MoveSpecification>() : line.Moves.ToList(),
                        current.ReqType != "F" && current.StoresFunction.ToLocationRequiredOrOptional());
                    await this.AddRequisitionLine(current, line);
                }
            }
        }
        
        // This Validation method ensures reqs are created in valid states. It is called in two scenarios:
        // 1. Client-side previewing of req validation during creation to provide feedback for the UI.
        // 2. Final server-side validation before persisting the req.
        //
        // While the RequisitionHeader class encapsulates much of its own validation logic (see RequisitionHeader.Validate()),
        // this manager method performs required additional checks, e.g:
        // - Calls stored procedures and accesses repositories as needed.
        // - Performs validations that must run only after lines are added (lines may be added by stored procedures for example).
        public async Task<RequisitionHeader> Validate(
            int createdBy,
            string functionCode,
            string reqType,
            int? document1Number,
            string document1Type,
            string departmentCode,
            string nominalCode,
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
            int? document1Line = null,
            string newPartNumber = null,
            IEnumerable<LineCandidate> lines = null,
            string isReverseTransaction = "N",
            int? originalDocumentNumber = null,
            IEnumerable<BookInOrderDetail> bookInOrderDetails = null,
            DateTime? dateReceived = null,
            string fromCategory = null,
            string auditLocation = null)
        {
            // just try and construct a req with a single line
            // exceptions will be thrown if any of the validation fails
            var function = !string.IsNullOrEmpty(functionCode)
                               ? await this.storesFunctionRepository.FindByIdAsync(functionCode)
                               : null;
            var dept = !string.IsNullOrEmpty(departmentCode)
                           ? await this.departmentRepository.FindByIdAsync(departmentCode)
                           : null;
            var nom = !string.IsNullOrEmpty(nominalCode)
                          ? await this.nominalRepository.FindByIdAsync(nominalCode)
                          : null;
            var fromLocation = !string.IsNullOrEmpty(fromLocationCode)
                                   ? await this.storageLocationRepository.FindByAsync(x =>
                                         x.LocationCode == fromLocationCode)
                                   : null;

            var toLocation = !string.IsNullOrEmpty(toLocationCode)
                                 ? await this.storageLocationRepository.FindByAsync(x =>
                                       x.LocationCode == toLocationCode)
                                 : null;
            var part = !string.IsNullOrEmpty(partNumber) ? await this.partRepository.FindByIdAsync(partNumber) : null;

            var employee = await this.employeeRepository.FindByIdAsync(createdBy);

            RequisitionHeader toBeReversed = null;

            if (originalDocumentNumber.HasValue)
            {
                toBeReversed = await this.repository.FindByIdAsync(originalDocumentNumber.Value);
            }

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
                batchDate,
                null,
                null,
                null,
                isReverseTransaction ?? "N",
                toBeReversed,
                dateReceived,
                fromCategory,
                auditLocation);
            
            if (functionCode == "LOAN OUT" || functionCode == "LOAN BACK")
            {
                if (functionCode == "LOAN BACK" && (!document1Line.HasValue || document1Line == 0))
                {
                    throw new CreateRequisitionException("Specify a loan line");
                }

                if (isReverseTransaction != "Y")
                {
                    await this.CheckLoan(document1Number, lines, document1Line, quantity);
                }
                
                return req;
            }

            if (!string.IsNullOrEmpty(auditLocation))
            {
                var location = await this.auditLocationRepository.FindByAsync(a => a.StoragePlace == auditLocation);
                if (location == null)
                {
                    throw new CreateRequisitionException($"Audit Location {auditLocation} is not valid.");
                }
            }

            var lineCandidates = lines?.ToList();
            if (function.LinesRequired == "Y" && (lines == null || !lineCandidates.Any()))
            {
                throw new CreateRequisitionException($"Lines are required for {functionCode}");
            }

            if (lines != null)
            {   
                // LDREQ etc works with a to location but also needs a stock pool to avoid making stock locators with no stock pool
                var headerSpecifiesOnto = (req.ToPalletNumber.HasValue || req.ToLocation != null);
                var headerSpecifiesOntoStockPool = !string.IsNullOrEmpty(req.ToStockPool);
                foreach (var candidate in lineCandidates)
                {
                    req.AddLine(await this.ValidateLineCandidate(
                        candidate, 
                        req.StoresFunction, 
                        req.ReqType, 
                        headerSpecifiesOnto,
                        headerSpecifiesOntoStockPool));
                    
                    // need to run the moves validation separately if the header specifies the onto info
                    if (headerSpecifiesOnto && !(candidate.Moves?.Count() >= 1))
                    {
                        var moves = new List<MoveSpecification>
                        {
                            new MoveSpecification
                            {
                                Qty = candidate.Qty,
                                FromState = req.FromState,
                                FromPallet = req.FromPalletNumber,
                                ToLocation = req.ToLocation?.LocationCode,
                                ToLocationId = req.ToLocation?.LocationId,
                                ToPallet = req.ToPalletNumber,
                                ToStockPool = req.ToStockPool,
                                ToState = req.ToState,
                                FromStockPool = req.FromStockPool,
                                FromLocation = req.FromLocation?.LocationCode
                            }
                        };
                        await this.CheckMoves(
                            candidate.PartNumber, moves, reqType != "F" && req.StoresFunction.ToLocationRequiredOrOptional());
                    }
                }
            }
            
            switch (function.PartSource)
            {
                case "PO":
                    await this.CheckPurchaseOrder(
                        req,
                        bookInOrderDetails);
                    break;

                case "RO":
                    await this.CheckReturnsOrder(req);
                    break;

                case "WO":
                    await this.CheckValidWorksOrder(
                        document1Number,
                        part,
                        isReverseTransaction,
                        quantity);
                    break;

                case "C" when function.Document1Required():
                    await this.CheckCreditNote(req);
                    break;
            }

            if (req.IsReverseTrans() && req.Quantity != null)
            {
                if (req.OriginalReqNumber.HasValue)
                {
                    DoProcessResultCheck(
                        await this.storesService.ValidReverseQuantity(req.OriginalReqNumber.Value, req.Quantity.Value));
                }
            }
            
            if (req.Part == null && req.Lines.Count == 0 && function.PartSource != "C" && function.LinesRequired != "N")
            {
                throw new RequisitionException("Lines are required if header does not specify part");
            }

            if (function.AutomaticFunctionType() && req.Lines.Count == 0)
            {
                if (req.StoresFunction.PartNumberRequired() && req.Part == null)
                {
                    throw new RequisitionException("Part number is required");
                }

                if (req.StoresFunction.IsQuantityRequiredOrOptional() && req.Quantity == null)
                {
                    throw new RequisitionException("A quantity must be entered");
                }

                if (isReverseTransaction != "Y" && req.StoresFunction.FromLocationRequiredOrOptional() && req.FromLocation == null
                    && req.FromPalletNumber == null)
                {
                    throw new RequisitionException("A from location or pallet is required");
                }

                if (req.StoresFunction.FromStockPoolRequiredOrOptional()
                    && string.IsNullOrEmpty(req.FromStockPool) && req.StoresFunction.FunctionCode != "SUKIT")
                {
                    throw new RequisitionException("A from stock pool is required");
                }

                if (req.StoresFunction.FromStateRequiredOrOptional() && string.IsNullOrEmpty(req.FromState)
                    && !req.IsReverseTrans())
                {
                    throw new RequisitionException("A from state is required");
                }

                if (isReverseTransaction != "Y" 
                    && req.StoresFunction.ToLocationRequiredOrOptional() && req.ToLocation == null
                    && req.ToPalletNumber == null)
                {
                    throw new RequisitionException("A to location or pallet is required");
                }

                if (req.ToStockPoolRequiredWithPart()
                    && string.IsNullOrEmpty(req.ToStockPool))
                {
                    throw new RequisitionException("A to stock pool is required");
                }

                if (req.StoresFunction.ToStateRequiredOrOptional() && string.IsNullOrEmpty(req.ToState))
                {
                    throw new RequisitionException("A to state is required");
                }
                
                if (req.StoresFunction.NewPartNumberRequired())
                {
                    var newPart = !string.IsNullOrEmpty(newPartNumber)
                                      ? await this.partRepository.FindByIdAsync(newPartNumber)
                                      : null;
                    DoProcessResultCheck(this.storesService.ValidPartNumberChange(part, newPart));
                }

                await this.DoChecksForAutoHeader(req);
            }

            return req;
        }

        public async Task<RequisitionLine> ValidateLineCandidate(
            LineCandidate candidate,
            StoresFunction storesFunction = null,
            string reqType = null,
            bool headerSpecifiesOntoLocation = false,
            bool headerSpecifiesOntoStockPool = false)
        {
            var part = !string.IsNullOrEmpty(candidate?.PartNumber)
                ? await this.partRepository.FindByIdAsync(candidate.PartNumber)
                : null;

            var transactionDefinition = !string.IsNullOrEmpty(candidate?.TransactionDefinition)
                                            ? await this.transactionDefinitionRepository.FindByIdAsync(
                                                  candidate.TransactionDefinition)
                                            : null;

            var line = new RequisitionLine(
                0,
                candidate.LineNumber,
                part,
                candidate.Qty,
                transactionDefinition,
                candidate.Document1,
                candidate.Document1Line.GetValueOrDefault(),
                candidate.Document1Type);

            if (candidate.Moves != null && candidate.Moves.Any() && storesFunction != null)
            {
                var toLocationRequired = reqType != "F" && storesFunction.ToLocationRequiredOrOptional()
                                                        && storesFunction.FunctionCode != "AUDIT";
                await this.CheckMoves(
                    candidate.PartNumber,
                    candidate.Moves.ToList(),
                    toLocationRequired);
            }

            if (headerSpecifiesOntoLocation)
            {
                if (!headerSpecifiesOntoStockPool)
                {
                    throw new CreateRequisitionException(
                        "Must specify both header loc and stock pool for onto");
                }
                return line;
            }
            
            if ((candidate.Moves == null || !candidate.Moves.Any()) &&
                line.TransactionDefinition.RequiresOntoTransactions)
            {
                throw new CreateRequisitionException(
                    $"Must specify moves onto for {line.TransactionDefinition.TransactionCode}");
            }

            if (candidate.SerialNumbers != null && candidate.SerialNumbers.Any())
            {
                foreach (var serialNumber in candidate.SerialNumbers)
                {
                    line.AddSerialNumber(serialNumber);
                }
            }

            await this.ValidateLineSerialNumbers(line);

            return line;
        }

        public async Task<DocumentResult> GetDocument(string docName, int docNumber, int? lineNumber)
        {
            if (docName == "C")
            {
                var result = await this.documentProxy.GetCreditNote(docNumber, lineNumber);

                if (result == null)
                {
                    if (lineNumber.HasValue)
                    {
                        throw new DocumentException($"Could not find credit note {docNumber} with line {lineNumber}");
                    }

                    throw new DocumentException($"Could not find credit note {docNumber}");
                }

                if (lineNumber.HasValue && result.Quantity == 0)
                {
                    throw new DocumentException($"Credit note {docNumber} line {lineNumber} is cancelled");
                }

                return result;
            }

            return null;
        }

        public async Task CheckDocumentLineForOverAndFullyBooked(RequisitionHeader header, DocumentResult document)
        {
            if (header.HasDocument1WithLine() && document.Quantity.HasValue)
            {
                var reqs = await this.repository.FilterByAsync(r =>
                               r.Document1Name == header.Document1Name && r.Document1 == header.Document1.Value
                                                                       && r.Document1Line == header.Document1Line
                                                                       && r.Cancelled == "N" && r.Quantity != null);

                if (reqs.Any())
                {
                    var bookQty = reqs.Sum(r => r.Quantity.GetValueOrDefault());

                    if (!header.IsReverseTrans())
                    {
                        if (header.Quantity.HasValue)
                        {
                            if (bookQty + header.Quantity > document.Quantity)
                            {
                                throw new DocumentException("Trying to overbook this line");
                            }
                        }
                        else if (bookQty == document.Quantity)
                        {
                            throw new DocumentException("Line is already fully booked");
                        }
                    }
                    else if (bookQty == 0)
                    {
                        throw new DocumentException("Line has never been booked");
                    }
                }
            }
        }

        public async Task<IEnumerable<PotentialMoveDetail>> AddPotentialMoveDetails(
            string documentType,
            int documentId,
            decimal? quantity,
            string partNumber,
            int? builtById,
            int? toLocationId,
            int? toPalletNumber)
        {
            var potentialMoves =
                await this.potentialMoveRepository.FilterByAsync(
                    a => a.DocumentId == documentId && a.DocumentType == documentType);
            var seq = 1;
            if (potentialMoves != null && potentialMoves.Any())
            {
                seq = potentialMoves.Max(a => a.Sequence) + 1;
            }

            var potentialMove = new PotentialMoveDetail
                                    {
                                        PartNumber = partNumber,
                                        Quantity = quantity,
                                        BuiltBy = builtById,
                                        Sequence = seq,
                                        DocumentType = documentType,
                                        DocumentId = documentId,
                                        LocationId = toLocationId,
                                        PalletNumber = toPalletNumber
                                    };
            await this.potentialMoveRepository.AddAsync(potentialMove);

            return new List<PotentialMoveDetail> { potentialMove }.AsEnumerable();
        }

        public async Task AddBookInOrderDetails(IList<BookInOrderDetail> details)
        {
            if (details != null && details.Count > 0)
            {
                var first = details.First();
                var existing = await this.bookInOrderDetailRepository.FilterByAsync(a =>
                                   a.OrderNumber == first.OrderNumber && a.OrderLine == first.OrderLine);

                if (existing.Count > 0)
                {
                    foreach (var existingBookInOrderDetail in existing)
                    { 
                       this.bookInOrderDetailRepository.Remove(existingBookInOrderDetail);
                    }
                }

                foreach (var bookInOrderDetail in details)
                {
                    await this.bookInOrderDetailRepository.AddAsync(bookInOrderDetail);
                }
            }
        }

        public async Task CheckPurchaseOrderForOverAndFullyKitted(RequisitionHeader header, PurchaseOrderResult purchaseOrder)
        {
            if (header.Document1Name == "PO" && purchaseOrder != null && purchaseOrder.OrderQty(header.Document1Line).HasValue)
            {
                var reqs = await this.repository.FilterByAsync(r =>
                    r.Document1Name == header.Document1Name && r.Document1 == header.Document1.Value &&
                    r.Cancelled == "N" && r.Quantity != null && r.StoresFunction.FunctionCode == "SUKIT" && r.ReqNumber != header.ReqNumber);

                if (reqs.Any())
                {
                    var kittedQty = reqs.Sum(r => r.Quantity.GetValueOrDefault());

                    if (kittedQty >= purchaseOrder.OrderQty(header.Document1Line))
                    {
                        throw new DocumentException($"Full order qty {purchaseOrder.OrderQty(header.Document1Line)} on order {header.Document1} has already been kitted");
                    }

                    if (header.Quantity.HasValue)
                    {
                        if (kittedQty + header.Quantity > purchaseOrder.OrderQty(header.Document1Line))
                        {
                            throw new DocumentException($"Only {purchaseOrder.OrderQty(header.Document1Line) - kittedQty} left to kit for order {header.Document1}");
                        }
                    }
                }
            }
        }

        public async Task CheckReturnOrderForFullyBooked(RequisitionHeader header, PurchaseOrderResult purchaseOrder)
        {
            if (header.Document1Name == "RO" && purchaseOrder != null
                                             && purchaseOrder.OrderQty(header.Document1Line).HasValue)
            {
                // call to STORES_OO.QTY_RETURNED could rewrite in c# but a bit involved
                var qty = await this.requisitionStoredProcedures.GetQtyReturned(
                              header.Document1.Value,
                              header.Document1Line.Value);
                
                if (header.IsReverseTrans() && Math.Abs(header.Quantity.Value) > qty)
                {
                    throw new DocumentException($"Returns Order {header.Document1}/{header.Document1Line} has not yet been booked");
                }

                if (header.IsReverseTransaction != "Y" && header.Quantity.Value - qty <= 0)
                {
                    throw new DocumentException($"Returns Order {header.Document1}/{header.Document1Line} is fully booked");
                }
            }
        }

        public async Task ValidateLineSerialNumbers(RequisitionLine line)
        {
            var sernosOnLine = line.SerialNumbers != null && line.SerialNumbers.Any();

            if (string.IsNullOrEmpty(line.TransactionDefinition?.SernosTransCode))
            {
                // should not have serial numbers
                if (sernosOnLine)
                {
                    throw new SerialNumberException($"Serial numbers not required for line {line.LineNumber}");
                }
            }
            else if (line.Part != null)
            {
                var sernosRequired = await this.serialNumberService.GetSerialNumbersRequired(line.Part.PartNumber);

                if (sernosOnLine && !sernosRequired)
                {
                    throw new SerialNumberException($"Serial numbers not required for {line.Part.PartNumber}");
                }
                else if (sernosRequired && !line.IsCancelled() && !line.IsBooked())
                {
                    if (!sernosOnLine)
                    {
                        throw new SerialNumberException($"Serial numbers required for {line.Part.PartNumber}");
                    }
                    
                    // check serial numbers on line
                    foreach (var serialNumber in line.SerialNumbers)
                    {
                        var check = await this.serialNumberService.CheckSerialNumber(
                                        line.TransactionDefinition.SernosTransCode,
                                        line.Part.PartNumber,
                                        serialNumber.SerialNumber);
                        if (!check.Success)
                        {
                            throw new SerialNumberException(check.Message);
                        }
                    }

                    // check if enough serial numbers for part
                    if (line.SerialNumbers.Count != line.Qty)
                    {
                        throw new SerialNumberException($"Line {line.LineNumber} requires {line.Qty} serial numbers {line.SerialNumbers.Count} supplied");
                    }
                }
            }
        }

        private static void DoProcessResultCheck(ProcessResult result)
        {
            if (!result.Success)
            {
                throw new RequisitionException(result.Message);
            }
        }

        private async Task CheckPurchaseOrder(RequisitionHeader req, IEnumerable<BookInOrderDetail> bookInOrderDetails)
        {
            var po = await this.documentProxy.GetPurchaseOrder(req.Document1.GetValueOrDefault());

            if (po == null)
            {
                throw new CreateRequisitionException($"PO {req.Document1} does not exist!");
            }

            if (!po.IsAuthorised)
            {
                throw new CreateRequisitionException($"PO {req.Document1} is not authorised!");
            }

            if (po.IsFilCancelled)
            {
                throw new CreateRequisitionException($"PO {req.Document1} is FIL Cancelled!");
            }

            var orderRef = $"{po.DocumentType.Substring(0, 1)}{po.OrderNumber}";

            if (req.IsReverseTransaction != "Y" && req.StoresFunction.BatchRequired == "Y" && req.BatchRef != orderRef)
            {
                throw new CreateRequisitionException(
                    "You are trying to pass stock for payment from a different PO");
            }

            if (req.StoresFunction.FunctionCode == "SUKIT")
            {
                await this.CheckPurchaseOrderForOverAndFullyKitted(req, po);
            }

            if (req.StoresFunction.FunctionCode == "BOOKLD")
            {
                await this.CheckBookInOrderAndDetails(
                    req.Document1,
                    req.Quantity,
                    req.IsReverseTransaction,
                    req.Part,
                    bookInOrderDetails?.ToList());
            }

            if (req.StoresFunction.FunctionCode == "BOOKSU")
            {
                if (req.Part.StockControlled != "Y")
                {
                    throw new CreateRequisitionException(
                        $"{req.StoresFunction.FunctionCode} requires part to be stock controlled and {req.Part.PartNumber} is not.");
                }

                if (po.DocumentType == "CO")
                {
                    throw new CreateRequisitionException("Cannot book in parts against a credit order (CO).");
                }

                if (!req.IsReverseTrans())
                {
                    if (req.Quantity < 0)
                    {
                        throw new CreateRequisitionException(
                            $"Cannot book a negative quantity against order {req.Document1}.");
                    }

                    var qtyLeft = po.Details.First(a => a.Line == req.Document1Line).Deliveries
                        .Sum(a => a.QuantityOutstanding);

                    if (po.OverBookAllowed == "Y" && po.OverBookQty.HasValue)
                    {
                        qtyLeft += po.OverBookQty.Value;
                    }

                    if (qtyLeft < req.Quantity)
                    {
                        throw new CreateRequisitionException(
                            $"The qty remaining on order {req.Document1}/{req.Document1Line} is {qtyLeft}. Cannot book {req.Quantity}.");
                    }
                }
            }
        }

        private async Task CheckReturnsOrder(
            RequisitionHeader req)
        {
            var ro = await this.documentProxy.GetPurchaseOrder(req.Document1.GetValueOrDefault());

            if (ro == null)
            {
                throw new CreateRequisitionException(message: $"RO {req.Document1} does not exist!");
            }

            if (ro.DocumentType != "RO" && ro.DocumentType != "CO")
            {
                throw new CreateRequisitionException($"Order {req.Document1} is not a returns/credit order!");
            }

            await this.CheckReturnOrderForFullyBooked(req, ro);
        }

        private async Task CheckCreditNote(
            RequisitionHeader req)
        {
            if (req.Document1Name != "C")
            {
                throw new CreateRequisitionException("Function requires a credit note");
            }

            var document = await this.GetDocument(
                req.Document1Name,
                req.Document1.GetValueOrDefault(),
                req.Document1Line);

            await this.CheckDocumentLineForOverAndFullyBooked(req, document);
        }

        private async Task CheckBookInOrderAndDetails(
            int? document1Number,
            decimal? reqQuantity,
            string isReverseTransaction,
            Part part,
            IList<BookInOrderDetail> bookInOrderDetails)
        {
            if (part == null)
            {
                throw new CreateRequisitionException("A sundry part must be specified for BOOKLD");
            }

            if (part.StockControlled == "Y")
            {
                throw new CreateRequisitionException($"Part {part.PartNumber} is stock controlled and BOOKLD must be sundry.");
            }
            
            if (bookInOrderDetails == null || bookInOrderDetails.Count == 0)
            {
                throw new CreateRequisitionException("No book in order details supplied for BOOKLD transaction");
            }

            if (!reqQuantity.HasValue || reqQuantity.Value == 0
                                      || bookInOrderDetails.Any(b => !b.Quantity.HasValue)
                                      || bookInOrderDetails.Any(b => b.Quantity == 0))
            {
                throw new CreateRequisitionException($"You must specify a quantity on req and all lines to book {document1Number}.");
            }

            if (isReverseTransaction == "Y" && (reqQuantity > 0 || bookInOrderDetails.Any(a => a.Quantity > 0)))
            {
                throw new CreateRequisitionException($"You must specify a negative quantity for reverse but {reqQuantity} supplied.");
            }

            if (isReverseTransaction == "N" && (reqQuantity < 0 || bookInOrderDetails.Any(a => a.Quantity < 0)))
            {
                throw new CreateRequisitionException($"You must specify a positive quantity for BOOKLD but {reqQuantity} supplied.");
            }

            var bookInDetailsQuantity = bookInOrderDetails.Sum(b => b.Quantity);
            if (reqQuantity.Value != bookInDetailsQuantity)
            {
                throw new CreateRequisitionException($"Book in order detail quantity ({bookInDetailsQuantity}) does not match req quantity ({reqQuantity}).");
            }

            if (bookInOrderDetails.Any(p => p.PartNumber != part.PartNumber))
            {
                throw new CreateRequisitionException("Part number is missing or incorrect on book in order details.");
            }

            if (bookInOrderDetails.Any(p =>
                    string.IsNullOrEmpty(p.DepartmentCode) || string.IsNullOrEmpty(p.NominalCode)))
            {
                throw new CreateRequisitionException("Department or Nominal missing on book in order details.");
            }

            foreach (var bookInOrderDetail in bookInOrderDetails)
            {
                DoProcessResultCheck(
                    await this.storesService.ValidDepartmentNominal(
                        bookInOrderDetail.DepartmentCode,
                        bookInOrderDetail.NominalCode));
            }
        }

        private async Task CheckValidWorksOrder(
            int? document1Number,
            Part part,
            string isReverseTransaction,
            decimal? quantity)
        {
            var worksOrder = await this.documentProxy.GetWorksOrder(document1Number.GetValueOrDefault());

            if (worksOrder == null)
            {
                throw new CreateRequisitionException($"Works Order {document1Number} does not exist.");
            }

            if (!string.IsNullOrEmpty(worksOrder.DateCancelled))
            {
                throw new CreateRequisitionException($"Works Order {document1Number} is cancelled.");
            }

            if ((!quantity.HasValue || quantity.Value == 0) && isReverseTransaction != "Y")
            {
                throw new CreateRequisitionException($"You must specify a quantity to book in from works order {document1Number}.");
            }

            if (worksOrder.PartNumber != part.PartNumber)
            {
                throw new CreateRequisitionException($"Works Order {document1Number} is for a different part.");
            }

            if ((worksOrder.Quantity == 0 || worksOrder.Quantity <= worksOrder.QuantityBuilt) && isReverseTransaction != "Y")
            {
                throw new CreateRequisitionException($"There is nothing left to build on Works Order {document1Number}.");
            }

            if ((worksOrder.Quantity == 0 || worksOrder.QuantityBuilt == 0) && isReverseTransaction == "Y")
            {
                throw new CreateRequisitionException($"Nothing built on Works Order {document1Number} that can be reversed.");
            }

            var salesPart = await this.salesProxy.GetSalesArticle(worksOrder.PartNumber);
            if (salesPart != null && isReverseTransaction != "Y"
                                  && (salesPart.TypeOfSerialNumber == "S" || salesPart.TypeOfSerialNumber == "P1"
                                                                          || salesPart.TypeOfSerialNumber == "P2"))
            {
                throw new CreateRequisitionException("You cannot book serial numbered parts in. Please use the relevant works order screen.");
            }

            if (part.BomVerifyFreqWeeks.GetValueOrDefault() > 0)
            {
                var verifications = await this.bomVerificationProxy.GetBomVerifications(worksOrder.PartNumber);
                if (verifications == null || !verifications.Any())
                {
                    throw new CreateRequisitionException($"Part number {worksOrder.PartNumber} requires bom verification.");
                }

                var latestDate = verifications.Max(a => a.DateVerified);
                var dateDue = latestDate.AddDays(part.BomVerifyFreqWeeks.GetValueOrDefault() * 7);
                if (dateDue < DateTime.Today)
                {
                    throw new CreateRequisitionException($"Part number {worksOrder.PartNumber} was due for bom verification on {dateDue:dd-MMM-yyyy}.");
                }
            }
        }

        private async Task CheckMoves(
            string partNumber,
            IList<MoveSpecification> moves,
            bool toLocationRequired = false)
        {
            foreach (var m in moves)
            {
                if (m.Qty <= 0)
                {
                    throw new RequisitionException("Move qty is invalid");
                }

                if (!m.ToPallet.HasValue && string.IsNullOrEmpty(m.ToLocation) && !m.FromPallet.HasValue
                    && string.IsNullOrEmpty(m.FromLocation))
                {
                    throw new RequisitionException("Moves are missing location information");
                }

                if (!m.ToPallet.HasValue && string.IsNullOrEmpty(m.ToLocation) && toLocationRequired)
                {
                    throw new RequisitionException("You must provide a to location or pallet");
                }

                if (!string.IsNullOrEmpty(m.FromLocation) || m.FromPallet.HasValue)
                {
                    int? locationId = null;
                    if (!string.IsNullOrEmpty(m.FromLocation))
                    {
                        var location =
                            await this.storageLocationRepository.FindByAsync(a => a.LocationCode == m.FromLocation);
                        if (location == null)
                        {
                            throw new RequisitionException($"From location {m.FromLocation} does not exist.");
                        }

                        locationId = location.LocationId;
                    }

                    if (m.IsAddition)
                    {
                        DoProcessResultCheck(
                            await this.stockService.ValidStockLocation(
                                locationId,
                                m.FromPallet,
                                partNumber,
                                m.Qty,
                                m.FromState,
                                m.FromStockPool));
                    }
                }

                // just checking moves onto for now, but could extend if required
                if (m.ToPallet.HasValue || !string.IsNullOrEmpty(m.ToLocation))
                {
                    if ((m.ToPallet.HasValue && !string.IsNullOrEmpty(m.ToLocation))
                        || (!m.ToPallet.HasValue && string.IsNullOrEmpty(m.ToLocation)))
                    {
                        throw new InsertReqOntosException(
                            "Move onto must specify either location code or pallet number");
                    }

                    if (m.ToPallet.HasValue)
                    {
                        var pallet = await this.palletRepository.FindByIdAsync(m.ToPallet.Value);

                        if (pallet == null || pallet.DateInvalid.HasValue)
                        {
                            throw new InsertReqOntosException($"Pallet {m.ToPallet.Value} is invalid");
                        }

                        var canPutPartOnPallet = await this.requisitionStoredProcedures.CanPutPartOnPallet(
                                                     partNumber,
                                                     m.ToPallet.Value);
                        if (!canPutPartOnPallet)
                        {
                            throw new CannotPutPartOnPalletException(
                                $"Cannot put part {partNumber} onto P{m.ToPallet}");
                        }
                    }
                    else if (!string.IsNullOrEmpty(m.ToLocation))
                    {
                        var toLocation =
                            await this.storageLocationRepository.FindByAsync(x => x.LocationCode == m.ToLocation);
                        if (toLocation == null)
                        {
                            throw new InsertReqOntosException($"Location {m.ToLocation} not found");
                        }

                        var part = await this.partRepository.FindByIdAsync(partNumber);
                        var state = await this.stateRepository.FindByIdAsync(m.ToState);

                        DoProcessResultCheck(await this.storesService.ValidOntoLocation(part, toLocation, null, state));

                        m.ToLocationId = toLocation.LocationId;
                    }
                }
            }
        }

        private async Task DoChecksForAutoHeader(RequisitionHeader header)
        {
            StoresPallet toPallet = null;
            if (header.ToPalletNumber.HasValue)
            {
                toPallet = await this.palletRepository.FindByIdAsync(header.ToPalletNumber.Value);
                if (toPallet == null || toPallet.DateInvalid.HasValue)
                {
                    throw new RequisitionException($"Pallet number {header.ToPalletNumber} is invalid");
                }
            }

            var toState = await this.stateRepository.FindByIdAsync(header.ToState);
            if (!string.IsNullOrEmpty(header.ToState) && toState == null)
            {
                throw new RequisitionException($"To State {header.ToState} does not exist");
            }

            var stockPool = await this.stockPoolRepository.FindByIdAsync(header.ToStockPool);
            if (!string.IsNullOrEmpty(header.ToStockPool) && stockPool == null)
            {
                throw new RequisitionException($"To Stock Pool {header.ToStockPool} does not exist");
            }

            if (header.Part != null && header.IsReverseTransaction != "Y" && header.StoresFunction?.ToLocationRequired != "N")
            {
                DoProcessResultCheck(await this.storesService.ValidOntoLocation(
                    header.Part,
                    header.ToLocation,
                    toPallet,
                    toState));
            }

            if (!string.IsNullOrEmpty(header.FromState))
            {
                DoProcessResultCheck(await this.storesService.ValidState(
                    null,
                    header.StoresFunction,
                    header.FromState,
                    "F"));
            }

            if (!string.IsNullOrEmpty(header.ToState))
            {
                DoProcessResultCheck(await this.storesService.ValidState(
                    null,
                    header.StoresFunction,
                    header.ToState,
                    "O"));
            }

            if (!string.IsNullOrEmpty(header.ToStockPool))
            {
                DoProcessResultCheck(this.storesService.ValidStockPool(header.Part, stockPool));
            }

            if (header.Part != null && !header.IsReverseTrans())
            {
                if (header.FromLocation != null || header.FromPalletNumber.HasValue)
                {
                    DoProcessResultCheck(
                        await this.stockService.ValidStockLocation(
                            header.FromLocation?.LocationId,
                            header.FromPalletNumber,
                            header.Part?.PartNumber,
                            header.Quantity.GetValueOrDefault(),
                            header.FromState));
                }
            }
        }

        private async Task CheckLoan(int? loanNumber, IEnumerable<LineCandidate> reqLines, int? lineNumber = null, decimal? headerQty = null)
        {
            // check req header values against loan
            if (!loanNumber.HasValue)
            {
                throw new CreateRequisitionException("No loan number specified");
            }

            var loan = await this.documentProxy.GetLoan(loanNumber.Value);
            if (loan == null)
            {
                throw new CreateRequisitionException($"Loan Number {loanNumber} does not exist");
            }

            if (loan.IsCancelled)
            {
                throw new CreateRequisitionException($"Loan Number {loanNumber} is cancelled");
            }

            if (lineNumber.HasValue)
            {
                var loanLine = loan.Details.SingleOrDefault(x => x.LineNumber == lineNumber.Value);

                if (loanLine == null)
                {
                    throw new CreateRequisitionException($"Loan Number {loanNumber} does not have a line number {lineNumber.Value}");
                }

                if (headerQty.HasValue && loanLine.Quantity != headerQty.Value)
                {
                    throw new CreateRequisitionException($"Loan line {loanNumber}/{lineNumber} is for a qty of {headerQty}");
                }
            }

            // check the parts/qties on req lines match the loan
            var lineCandidates = reqLines?.ToList();

            if (reqLines != null && lineCandidates.Count != 0)
            {
                if (lineCandidates.GroupBy(x => x.Document1Line).Count() != lineCandidates.Count())
                {
                    throw new RequisitionException("Each req line must specify a different loan line");
                }

                foreach (var l in lineCandidates)
                {
                    var loanLine = loan.Details.SingleOrDefault(x => x.LineNumber == l.Document1Line);

                    if (loanLine == null)
                    {
                        throw new RequisitionException($"Loan line {l.Document1Line} does not exist");
                    }

                    if (l.Cancelled != "Y" && loanLine.IsCancelled)
                    {
                        throw new RequisitionException($"Loan line {l.Document1Line} is cancelled");
                    }

                    if (loanLine.Quantity != l.Qty)
                    {
                        throw new RequisitionException(
                            $"Loan line {l.Document1Line} is for a qty of {loanLine.Quantity}");
                    }

                    if (loanLine.ArticleNumber != l.PartNumber)
                    {
                        throw new RequisitionException($"Loan line {l.Document1Line} is for {loanLine.ArticleNumber}");
                    }
                }
            }
        }
    }
}
