namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Resources.Imports;
    using Linn.Stores2.Service.Extensions;
    using Linn.Stores2.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class ImportBookExchangeRateModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/import-book-exchange-rates", this.GetApp);
            app.MapGet("/stores2/import-book-exchange-rates/by-period", this.GetByPeriod);
            app.MapGet("/stores2/import-book-exchange-rates/rate", this.GetByKey);
            app.MapPost("/stores2/import-book-exchange-rates", this.Create);
            app.MapPut("/stores2/import-book-exchange-rates", this.Update);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task GetByPeriod(
            HttpResponse res,
            int periodNumber,
            IAsyncFacadeService<ImportBookExchangeRate, ImportBookExchangeRateKey, ImportBookExchangeRateResource, ImportBookExchangeRateResource, ImportBookExchangeRateResource> facadeService)
        {
            var searchResource = new ImportBookExchangeRateResource { PeriodNumber = periodNumber };
            await res.Negotiate(await facadeService.FilterBy(searchResource));
        }

        private async Task GetByKey(
            HttpResponse res,
            int periodNumber,
            string baseCurrency,
            string exchangeCurrency,
            IAsyncFacadeService<ImportBookExchangeRate, ImportBookExchangeRateKey, ImportBookExchangeRateResource, ImportBookExchangeRateResource, ImportBookExchangeRateResource> facadeService)
        {
            var key = new ImportBookExchangeRateKey
            {
                PeriodNumber = periodNumber,
                BaseCurrency = baseCurrency,
                ExchangeCurrency = exchangeCurrency
            };
            await res.Negotiate(await facadeService.GetById(key));
        }

        private async Task Create(
            HttpRequest req,
            HttpResponse res,
            ImportBookExchangeRateResource resource,
            IUserPrivilegeService userPrivilegeService,
            IAsyncFacadeService<ImportBookExchangeRate, ImportBookExchangeRateKey, ImportBookExchangeRateResource, ImportBookExchangeRateResource, ImportBookExchangeRateResource> facadeService)
        {
            var privileges = await userPrivilegeService.GetUserPrivileges(req.HttpContext.User.GetEmployeeUrl());
            await res.Negotiate(await facadeService.Add(resource, privileges));
        }

        private async Task Update(
            HttpRequest req,
            HttpResponse res,
            int periodNumber,
            string baseCurrency,
            string exchangeCurrency,
            ImportBookExchangeRateResource resource,
            IUserPrivilegeService userPrivilegeService,
            IAsyncFacadeService<ImportBookExchangeRate, ImportBookExchangeRateKey, ImportBookExchangeRateResource, ImportBookExchangeRateResource, ImportBookExchangeRateResource> facadeService)
        {
            var key = new ImportBookExchangeRateKey
            {
                PeriodNumber = periodNumber,
                BaseCurrency = baseCurrency,
                ExchangeCurrency = exchangeCurrency
            };
            resource.PeriodNumber = periodNumber;
            resource.BaseCurrency = baseCurrency;
            resource.ExchangeCurrencyCode = exchangeCurrency;
            var privileges = await userPrivilegeService.GetUserPrivileges(req.HttpContext.User.GetEmployeeUrl());
            await res.Negotiate(await facadeService.Update(key, resource, privileges));
        }
    }
}
