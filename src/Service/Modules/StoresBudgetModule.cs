namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Resources.RequestResources;
    using Linn.Stores2.Resources.Stores;
    using Linn.Stores2.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class StoresBudgetModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/budgets", this.GetApp);
            app.MapGet("/stores2/budgets/{id:int}", this.GetById);
            app.MapGet("/stores2/budgets/search", this.Search);
        }

        private async Task GetById(
            HttpRequest _, 
            HttpResponse res, 
            int id,
            IAsyncFacadeService<StoresBudget, int, StoresBudgetResource, StoresBudgetResource, StoresBudgetSearchResource> service)
        {
            var result = await service.GetById(id);
            await res.Negotiate(result);
        }

        private async Task Search(
            HttpRequest req,
            HttpResponse res,
            string fromDate,
            string toDate,
            string partNumber,
            string partNumberStartsWith,
            IAsyncFacadeService<StoresBudget, int, StoresBudgetResource, StoresBudgetResource, StoresBudgetSearchResource> service)
        {
            var searchResource = new StoresBudgetSearchResource
                                     {
                                         FromDate = fromDate,
                                         ToDate = toDate,
                                         PartNumber = partNumber,
                                         PartNumberStartsWith = partNumberStartsWith
                                     };
            var result = await service.FilterBy(searchResource);
            await res.Negotiate(result);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }
    }
}
