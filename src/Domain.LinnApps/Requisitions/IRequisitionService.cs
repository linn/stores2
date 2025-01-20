namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Threading.Tasks;

    public interface IRequisitionService
    {
        Task<RequisitionHeader> Cancel(
            int reqNumber, 
            User cancelledBy,
            string reason);
    }
}
