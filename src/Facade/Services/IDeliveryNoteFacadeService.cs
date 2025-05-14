using System.Threading.Tasks;

namespace Linn.Stores2.Facade.Services
{
    public interface IDeliveryNoteFacadeService
    {
        Task<string> GetDeliveryNoteAsHtml(
            int reqNumber);
    }
}
