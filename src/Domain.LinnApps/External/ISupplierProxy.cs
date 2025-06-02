namespace Linn.Stores2.Domain.LinnApps.External
{
    using System.Threading.Tasks;

    public interface ISupplierProxy
    {
        Task<Address> GetSupplierAddress(int supplierId);
    }
}
