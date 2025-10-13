namespace Linn.Stores2.Service.ResultHandlers
{
    using Linn.Common.Service.Handlers;
    using Linn.Stores2.Resources.Requisitions;

    public class RequisitionApplicationStateResultHandler : JsonResultHandler<RequisitionHeaderResource>
    {
        public RequisitionApplicationStateResultHandler() : base("application/vnd.linn.application-state+json")
        {
        }
    }
}
