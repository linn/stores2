namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Threading.Tasks;

    public interface IRequisitionManager
    {
        Task<RequisitionHeader> CancelHeader(
            int reqNumber, 
            User cancelledBy,
            string reason,
            bool requiresAuth = true);

        Task<RequisitionHeader> CancelLine(
            int reqNumber,
            int lineNumber,
            User cancelledBy,
            string reason);

        Task<RequisitionHeader> BookRequisition(
            int reqNumber,
            int? lineNumber,
            User bookedBy);

        Task AddRequisitionLine(RequisitionHeader header, LineCandidate toAdd);
    }
}
