namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Resources.Stores;

    public class StoresBudgetResourceBuilder : IBuilder<StoresBudget>
    {
        private readonly IAuthorisationService authService;

        public StoresBudgetResourceBuilder(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public StoresBudgetResource Build(StoresBudget storesBudget, IEnumerable<string> claims)
        {
            if (storesBudget == null)
            {
                return null;
            }
            
            var reqLineBuilder = new RequisitionLineResourceBuilder();
            var reqBuilder = new RequisitionResourceBuilder(this.authService);
            var builder = new StoresBudgetResourceWithoutReqLineBuilder();
            var claimsList = claims?.ToList();

            var resource = builder.Build(storesBudget, null);
            if (storesBudget.RequisitionLine != null)
            {
                resource.RequisitionLine = reqLineBuilder.Build(storesBudget.RequisitionLine, null);
                if (storesBudget.RequisitionLine.RequisitionHeader != null)
                {
                    resource.RequisitionLine.RequisitionHeader =
                        reqBuilder.Build(storesBudget.RequisitionLine.RequisitionHeader, null);
                }
            }

            resource.Links = this.BuildLinks(storesBudget, claimsList).ToArray();

            return resource;
        }

        public string GetLocation(StoresBudget model)
        {
            return $"/stores2/budgets/{model.BudgetId}";
        }

        object IBuilder<StoresBudget>.Build(StoresBudget entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(StoresBudget model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
