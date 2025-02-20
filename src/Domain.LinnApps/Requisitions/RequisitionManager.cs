namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Logging;
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

        private readonly ILog logger;

        private readonly IStoresService storesService;

        private readonly IRepository<StoresPallet, int> palletRepository;

        private readonly IRepository<StockState, string> stateRepository;

        public RequisitionManager(
            IAuthorisationService authService,
            IRepository<RequisitionHeader, int> repository,
            IRequisitionStoredProcedures requisitionStoredProcedures,
            IRepository<Employee, int> employeeRepository,
            IRepository<Part, string> partRepository,
            IRepository<StorageLocation, int> storageLocationRepository,
            IRepository<StoresTransactionDefinition, string> transactionDefinitionRepository,
            ITransactionManager transactionManager,
            ILog logger,
            IStoresService storesService,
            IRepository<StoresPallet, int> palletRepository,
            IRepository<StockState, string> stateRepository)
        {
            this.authService = authService;
            this.repository = repository;
            this.requisitionStoredProcedures = requisitionStoredProcedures;
            this.employeeRepository = employeeRepository;
            this.partRepository = partRepository;
            this.storageLocationRepository = storageLocationRepository;
            this.transactionDefinitionRepository = transactionDefinitionRepository;
            this.transactionManager = transactionManager;
            this.logger = logger;
            this.storesService = storesService;
            this.palletRepository = palletRepository;
            this.stateRepository = stateRepository;
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

            foreach (var pick in toAdd.StockPicks)
            {
                var fromLocation = string.IsNullOrEmpty(pick.Location)
                    ? null
                    : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == pick.Location);
                var pickResult = await this.requisitionStoredProcedures.PickStock(
                                     toAdd.PartNumber,
                                     header.ReqNumber,
                                     toAdd.LineNumber,
                                     pick.Qty,
                                     fromLocation?.LocationId, // todo - do we pass a value here if palletNumber?
                                     pick.Pallet,
                                     header.FromStockPool,
                                     toAdd.TransactionDefinition);

                if (!pickResult.Success)
                {
                    throw new PickStockException("failed in pick_stock: " + pickResult.Message);
                }
            }

            foreach (var moveOnto in toAdd.MovesOnto)
            {
                if (moveOnto.Pallet.HasValue)
                {
                    var canPutPartOnPallet = await this.requisitionStoredProcedures.CanPutPartOnPallet(
                        toAdd.PartNumber,
                        moveOnto.Pallet.Value);

                    if (!canPutPartOnPallet)
                    {
                        throw new CannotPutPartOnPalletException(
                            $"Cannot put part {toAdd.PartNumber} onto P{moveOnto.Pallet}");
                    }
                }

                var toLocation = string.IsNullOrEmpty(moveOnto.Location)
                                       ? null
                                       : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == moveOnto.Location);
                if (toLocation == null)
                {
                    throw new InsertReqOntosException($"Did not recognise location {moveOnto.Location}");
                }

                var insertOntosResult = await this.requisitionStoredProcedures.InsertReqOntos(
                                            header.ReqNumber,
                                            moveOnto.Qty,
                                            toAdd.LineNumber,
                                            toLocation.LocationId,
                                            moveOnto.Pallet,
                                            header.ToStockPool,
                                            header.ToState,
                                            "FREE");

                if (!insertOntosResult.Success)
                {
                    throw new InsertReqOntosException($"Failed in insert_req_ontos: {insertOntosResult.Message}");
                }
            }

            // I'm assuming this knows which way round the debits and credits go based on the direction of the req?
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

            var checkOnto = await this.storesService.ValidOntoLocation(
                                header.Part,
                                header.ToLocation,
                                toPallet,
                                toState);
            if (!checkOnto.Success)
            {
                throw new RequisitionException(checkOnto.Message);
            }

            await this.repository.AddAsync(header);

            var proxyResult =
                await this.requisitionStoredProcedures.CreateRequisitionLines(header.ReqNumber, null);

            if (!proxyResult.Success)
            {
                throw new RequisitionException(proxyResult.Message);
            }


            proxyResult = await this.requisitionStoredProcedures.CanBookRequisition(
                              header.ReqNumber,
                              null,
                              header.Quantity.GetValueOrDefault());

            if (!proxyResult.Success)
            {
                throw new RequisitionException(proxyResult.Message);
            }

            proxyResult = await this.requisitionStoredProcedures.DoRequisition(
                              header.ReqNumber,
                              null,
                              header.CreatedBy.Id);

            if (!proxyResult.Success)
            {
                throw new RequisitionException(proxyResult.Message);
            }
        }
    }
}
