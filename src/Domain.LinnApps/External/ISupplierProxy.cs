using System.Threading.Tasks;

namespace Linn.Stores2.Domain.LinnApps.External
{
    public interface ISupplierProxy
    {
        Task<Address> GetSupplierAddress(int supplierId);
    }
}
