namespace Linn.Stores2.Facade.Services
{
    using System.Threading.Tasks;

    public interface IDeliveryNoteFacadeService
    {
        Task<string> GetDeliveryNoteAsHtml(int reqNumber);
    }
}
