namespace Linn.Stores2.Facade.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.Requisitions;

    public interface IRequisitionFacadeService 
        : IAsyncFacadeService<RequisitionHeader, int, RequisitionHeaderResource,
        RequisitionHeaderResource, RequisitionSearchResource>
    {
        Task<IResult<RequisitionHeaderResource>> CancelHeader(
            int reqNumber, int cancelledBy, string reason, IEnumerable<string> privileges);
    }
}
