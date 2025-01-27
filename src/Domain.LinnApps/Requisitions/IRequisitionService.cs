namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Threading.Tasks;

    public interface IRequisitionService
    {
        Task<RequisitionHeader> CancelHeader(
            int reqNumber, 
            User cancelledBy,
            string reason);

        Task<RequisitionHeader> CancelLine(
            int reqNumber,
            int lineNumber,
            User cancelledBy,
            string reason);
    }
}
