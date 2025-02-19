namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRequisitionManager
    {
        Task<RequisitionHeader> CancelHeader(
            int reqNumber, 
            int cancelledBy,
            IEnumerable<string> privileges,
            string reason,
            bool requiresAuth = true);

        Task<RequisitionHeader> CancelLine(
            int reqNumber,
            int lineNumber,
            int cancelledBy,
            IEnumerable<string> privileges,
            string reason);

        Task<RequisitionHeader> BookRequisition(
            int reqNumber,
            int? lineNumber,
            int bookedBy,
            IEnumerable<string> privileges);

        Task<RequisitionHeader> AuthoriseRequisition(
            int reqNumber,
            int authorisedBy,
            IEnumerable<string> privileges);


        Task AddRequisitionLine(RequisitionHeader header, LineCandidate toAdd);
    }
}
