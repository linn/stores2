namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Domain;
    using Linn.Common.Persistence;
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
            IRepository<StockPool, string> stockPoolRepository)
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

        public async Task AddRequisitionLine(RequisitionHeader header, LineCandidate toAdd)
        {
            var part = await this.partRepository.FindByIdAsync(toAdd.PartNumber);
            var transactionDefinition = await this.transactionDefinitionRepository.FindByIdAsync(toAdd.TransactionDefinition);

            header.AddLine(new RequisitionLine(header.ReqNumber, toAdd.LineNumber, part, toAdd.Qty, transactionDefinition));

            // we need this so the line exists for the stored procedure calls coming next
            await this.transactionManager.CommitAsync();

            if (toAdd.Moves != null)
            {
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
                }

                // write ons
                foreach (var moveOnto 
                         in toAdd.Moves.Where(x => x.ToPallet.HasValue || !string.IsNullOrEmpty(x.ToLocation)))
                {
                    if (moveOnto.ToPallet.HasValue)
                    {
                        var canPutPartOnPallet = await this.requisitionStoredProcedures.CanPutPartOnPallet(
                                                     toAdd.PartNumber,
                                                     moveOnto.ToPallet.Value);
                        if (!canPutPartOnPallet)
                        {
                            throw new CannotPutPartOnPalletException(
                                $"Cannot put part {toAdd.PartNumber} onto P{moveOnto.ToPallet}");
                        }
                    }

                    int? locationId = null;

                    if (!string.IsNullOrEmpty(moveOnto.ToLocation))
                    {
                        var toLocation = await this.storageLocationRepository.FindByAsync(x => x.LocationCode == moveOnto.ToLocation);
                        if (toLocation == null)
                        {
                            throw new InsertReqOntosException($"Did not recognise location {moveOnto.ToLocation}");
                        }

                        locationId = toLocation.LocationId;
                    }

                    var insertOntosResult = await this.requisitionStoredProcedures.InsertReqOntos(
                                                header.ReqNumber,
                                                moveOnto.Qty,
                                                toAdd.LineNumber,
                                                locationId,
                                                moveOnto.ToPallet,
                                                moveOnto.ToStockPool,
                                                moveOnto.ToState,
                                                "FREE"); // todo

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
                                           header.Nominal.NominalCode,
                                           header.Department.DepartmentCode);

            if (!createPostingsResult.Success)
            {
                throw new CreateNominalPostingException("failed in create_nominals: " + createPostingsResult.Message);
            }
        }

        public async Task CheckAndBookRequisitionHeader(RequisitionHeader header)
        {
            StoresPallet toPallet = null;
            if (header.ToPalletNumber.HasValue)
            {
                toPallet = await this.palletRepository.FindByIdAsync(header.ToPalletNumber.Value);
            }

            var toState = await this.stateRepository.FindByIdAsync(header.ToState);

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
            DoProcessResultCheck(this.storesService.ValidStockPool(header.Part, stockPool));

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
                    if (line.StockPicked.GetValueOrDefault() == true)
                    {
                        await this.PickStockOnRequisitionLine(current, line);
                    }
                    
                    // could support other line updates, e.g. updating other line fields here 
                }
                else
                {
                    // adding a new line
                    // note - this will pick stock/create ontos, create nominal postings etc
                    // might need to rethink if not all new lines need this behaviour (update strategies? some other pattern)
                    await this.AddRequisitionLine(current, line);
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
    }
}
