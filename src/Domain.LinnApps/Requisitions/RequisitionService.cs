namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;

    public class RequisitionService : IRequisitionService
    {
        private readonly IAuthorisationService authService;
        
        private readonly IRepository<RequisitionHeader, int> repository;

        private readonly IRequisitionStoredProcedures requisitionStoredProcedures;

        private readonly IRepository<Employee, int> employeeRepository;

        public RequisitionService(
            IAuthorisationService authService,
            IRepository<RequisitionHeader, int> repository,
            IRequisitionStoredProcedures requisitionStoredProcedures,
            IRepository<Employee, int> employeeRepository)
        {
            this.authService = authService;
            this.repository = repository;
            this.requisitionStoredProcedures = requisitionStoredProcedures;
            this.employeeRepository = employeeRepository;
        }
        
        public async Task<RequisitionHeader> CancelHeader(
            int reqNumber, 
            User cancelledBy,
            string reason)
        {
            if (!this.authService.HasPermissionFor(
                    AuthorisedActions.CancelRequisition, cancelledBy.Privileges))
            {
                throw new UnauthorizedAccessException(
                    "You do not have permission to cancel a requisition");
            }

            var req = await this.repository.FindByIdAsync(reqNumber);
            
            if (string.IsNullOrEmpty(req.Function.CancelFunction))
            {
                var by = await this.employeeRepository.FindByIdAsync(cancelledBy.UserNumber);
                req.Cancel(reason, by);
            }
            else if (req.Function.CancelFunction == "UNALLOC_REQ")
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
                throw new UnauthorizedAccessException(
                    "You do not have permission to cancel a requisition");
            }

            var req = await this.repository.FindByIdAsync(reqNumber);

            if (string.IsNullOrEmpty(req.Function.CancelFunction))
            {
                var by = await this.employeeRepository.FindByIdAsync(cancelledBy.UserNumber);
                req.CancelLine(lineNumber, reason, by);
            }
            else if (req.Function.CancelFunction == "UNALLOC_REQ")
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
                throw new UnauthorizedAccessException(
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
    }
}
