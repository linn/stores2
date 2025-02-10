namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class RequisitionService : IRequisitionService
    {
        private readonly IAuthorisationService authService;
        
        private readonly IRepository<RequisitionHeader, int> repository;

        private readonly IRequisitionStoredProcedures requisitionStoredProcedures;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<StoresFunction, string> storesFunctionRepository;

        private readonly IRepository<Department, string> departmentRepository;

        private readonly IRepository<Nominal, string> nominalRepository;

        private readonly IRepository<Part, string> partRepository;

        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        public RequisitionService(
            IAuthorisationService authService,
            IRepository<RequisitionHeader, int> repository,
            IRequisitionStoredProcedures requisitionStoredProcedures,
            IRepository<Employee, int> employeeRepository,
            IRepository<StoresFunction, string> storesFunctionRepository,
            IRepository<Department, string> departmentRepository,
            IRepository<Nominal, string> nominalRepository,
            IRepository<Part, string> partRepository,
            IRepository<StorageLocation, int> storageLocationRepository)
        {
            this.authService = authService;
            this.repository = repository;
            this.requisitionStoredProcedures = requisitionStoredProcedures;
            this.employeeRepository = employeeRepository;
            this.storesFunctionRepository = storesFunctionRepository;
            this.departmentRepository = departmentRepository;
            this.nominalRepository = nominalRepository;
            this.partRepository = partRepository;
            this.storageLocationRepository = storageLocationRepository;
        }
        
        public async Task<RequisitionHeader> CancelHeader(
            int reqNumber, 
            User cancelledBy,
            string reason)
        {
            if (!this.authService.HasPermissionFor(
                    AuthorisedActions.CancelRequisition, cancelledBy.Privileges))
            {
                throw new UnauthorisedActionException(
                    "You do not have permission to cancel a requisition");
            }

            var req = await this.repository.FindByIdAsync(reqNumber);
            
            if (string.IsNullOrEmpty(req.StoresFunction.CancelFunction))
            {
                var by = await this.employeeRepository.FindByIdAsync(cancelledBy.UserNumber);
                req.Cancel(reason, by);
            }
            else if (req.StoresFunction.CancelFunction == "UNALLOC_REQ")
            {
                var unallocateReqResult = await this.requisitionStoredProcedures.UnallocateRequisition(
                    reqNumber, null, cancelledBy.UserNumber);

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
                                             null,
                                             req.Document1,
                                             req.Document1Name);

            if (!deleteAllocsOntoResult.Success)
            {
                throw new RequisitionException(deleteAllocsOntoResult.Message);
            }

            return req;
        }

        public async Task<RequisitionHeader> CancelLine(
            int reqNumber, int lineNumber, User cancelledBy, string reason)
        {
            if (!this.authService.HasPermissionFor(
                    AuthorisedActions.CancelRequisition, cancelledBy.Privileges))
            {
                throw new UnauthorisedActionException(
                    "You do not have permission to cancel a requisition");
            }

            var req = await this.repository.FindByIdAsync(reqNumber);

            if (string.IsNullOrEmpty(req.StoresFunction.CancelFunction))
            {
                var by = await this.employeeRepository.FindByIdAsync(cancelledBy.UserNumber);
                req.CancelLine(lineNumber, reason, by);
            }
            else if (req.StoresFunction.CancelFunction == "UNALLOC_REQ")
            {
                var unallocateReqResult = await this.requisitionStoredProcedures.UnallocateRequisition(
                                              reqNumber, lineNumber, cancelledBy.UserNumber);

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

        public async Task<RequisitionHeader> BookRequisition(int reqNumber, int? lineNumber, User bookedBy)
        {
            if (!this.authService.HasPermissionFor(
                    AuthorisedActions.BookRequisition, bookedBy.Privileges))
            {
                throw new UnauthorisedActionException(
                    "You do not have permission to book a requisition");
            }

            var doRequisitionResult = await this.requisitionStoredProcedures.DoRequisition(
                reqNumber,
                lineNumber,
                bookedBy.UserNumber);

            if (!doRequisitionResult.Success)
            {
                throw new RequisitionException(doRequisitionResult.Message);
            }

            var req = await this.repository.FindByIdAsync(reqNumber);

            return req;
        }

        public async Task<RequisitionHeader> CreateRequisition(
            User createdBy,
            string functionCode,
            string reqType,
            int? document1Number,
            string document1Type,
            string departmentCode,
            string nominalCode,
            IEnumerable<LineCandidate> lines = null,
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
            decimal? qty = null)
        {
            // might make sense to have in IReqAuthService.HasPermissionFor(StoresFunction function, IEnumerable<string> privileges)
            // if this gets more complicated
            // so for now...
            if (functionCode == "LDREQ"
                && !this.authService.HasPermissionFor(AuthorisedActions.Ldreq, createdBy.Privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to raise LDREQ");
            }

            var who = await this.employeeRepository.FindByIdAsync(createdBy.UserNumber);
            var function = await this.storesFunctionRepository.FindByIdAsync(functionCode);
            var department = await this.departmentRepository.FindByIdAsync(departmentCode);
            var nominal = await this.nominalRepository.FindByIdAsync(nominalCode);
            var part = await this.partRepository.FindByIdAsync(partNumber);

            var fromLocation = string.IsNullOrEmpty(fromLocationCode)
                                   ? null
                                   : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == fromLocationCode);

            var toLocation = string.IsNullOrEmpty(fromLocationCode)
                                   ? null
                                   : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == toLocationCode);

            // todo - potentially some validation here
            // ideally most will be in the constructor called below, or if that gets to complicated we
            // delegate to factory class or similar?
            var req = new RequisitionHeader(
                who,
                function, 
                reqType, 
                document1Number,
                document1Type,
                department, 
                nominal,
                lines.Select(x => new RequisitionLine(null, x.LineNumber)), // todo - add lines properly
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
                qty);

            // todo - might need to commit here as req probably needs to exist for pick_stock to work
            // todo - call stores_oo.pick_stock for each line
            // todo - commit again
            // todo - call stores_oo.create_nominals to create nominal postings
            // todo - possibly some inserts into stores_move_logs (although could also try facade MaybeSaveToLogTable())
            // todo - fetch the req again with the real moves and the nominal postings .Include()'d
            return req;
        }
    }
}
