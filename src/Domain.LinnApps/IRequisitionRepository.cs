namespace Linn.Stores2.Domain.LinnApps
{
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public interface IRequisitionRepository : IRepository<RequisitionHeader, int>
    {
        Task<RequisitionHeader> FindByIdNoTracking(int id);
    }
}
