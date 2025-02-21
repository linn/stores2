namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    public interface ICreationStrategyResolver
    {
        ICreationStrategy Resolve(RequisitionCreationContext context);
    }
}
