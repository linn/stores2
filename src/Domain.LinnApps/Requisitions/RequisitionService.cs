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
        
        public async Task<RequisitionHeader> Cancel(
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

            if (req.DateBooked.HasValue)
            {
                throw new RequisitionException(
                    "Cannot cancel a requisition that has already been booked");
            }

            if (string.IsNullOrEmpty(req.FunctionCode.CancelFunction))
            {
                var by = await this.employeeRepository.FindByIdAsync(cancelledBy.UserNumber);
                req.Cancel(reason, by);
            }
            else if (req.FunctionCode.CancelFunction == "UNALLOC_REQ")
            {
                await this.requisitionStoredProcedures.UnallocateRequisition(
                    reqNumber, null, cancelledBy.UserNumber);
            }
            else
            {
                throw new RequisitionException(
                    "Cannot cancel req - invalid cancel function");
            }
            
            return req;
        }
    }
}
