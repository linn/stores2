namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Threading.Tasks;
    using Linn.Stores2.Domain.LinnApps.Models;

    public interface IDeliveryNoteService
    {
        Task<DeliveryNoteDocument> GetDeliveryNote(int reqNumber);
    }
}
