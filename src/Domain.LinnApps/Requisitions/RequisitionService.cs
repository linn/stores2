namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Exceptions;

    public class RequisitionService : IRequisitionService
    {
        private readonly IAuthorisationService authService;
        
        private readonly IRepository<RequisitionHeader, int> repository;

        public RequisitionService(
            IAuthorisationService authService,
            IRepository<RequisitionHeader, int> repository)
        {
            this.authService = authService;
            this.repository = repository;
        }
        
        public async Task<RequisitionHeader> Cancel(int reqNumber, User cancelledBy)
        {
            if (!this.authService.HasPermissionFor(AuthorisedActions.CancelRequisition, cancelledBy.Privileges))
            {
                throw new UnauthorizedAccessException("You do not have permission to cancel a requisition");
            }

            var req = await this.repository.FindByIdAsync(reqNumber);

            // doesn't make sense to have any cancellation logic inside the requisition class itself for now
            // since the actual cancel operations will be performed by executing a stored procedure
            if (req.DateBooked.HasValue)
            {
                throw new RequisitionException("Cannot cancel a requisition that has already been booked");
            }
            
            return req;
        }
    }
}
