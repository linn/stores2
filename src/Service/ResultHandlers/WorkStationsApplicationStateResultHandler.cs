namespace Linn.Stores2.Service.ResultHandlers
{
    using Linn.Common.Service.Handlers;
    using Linn.Stores2.Resources.Stores;

    public class WorkStationsApplicationStateResultHandler : JsonResultHandler<WorkStationResource>
    {
        public WorkStationsApplicationStateResultHandler() : base("application/vnd.linn.application-state+json")
        {
        }
    }
}
