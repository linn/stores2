namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System.Threading.Tasks;

    using Linn.Common.Persistence;

    public class PlaceholderStrategyForWhenHeaderHasPartNumber : ICreationStrategy
    {
        private readonly IRepository<RequisitionHeader, int> repository;

        private readonly IRequisitionManager requisitionManager;

        public PlaceholderStrategyForWhenHeaderHasPartNumber(
            IRepository<RequisitionHeader, int> repository,
            IRequisitionManager requisitionManager)
        {
            this.repository = repository;
            this.requisitionManager = requisitionManager;
        }

        public async Task Apply(RequisitionCreationContext context)
        {
            // just do the stuff Richard was doing when part number was set on the header
            await this.requisitionManager.CheckAndBookRequisitionHeader(context.Header);

            context.Header = await this.repository.FindByIdAsync(context.Header.ReqNumber);
        }
    }
}
