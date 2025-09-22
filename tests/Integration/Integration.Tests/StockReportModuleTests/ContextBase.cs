namespace Linn.Stores2.Integration.Tests.StockReportModuleTests
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Facade.Services;
    using System.Net.Http;
    using NUnit.Framework;
    using NSubstitute;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Service.Modules;
    using Microsoft.Extensions.DependencyInjection;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IStockReportFacadeService StockReportFacadeService { get; private set; }

        protected IStockReportService StockReportService { get; private set; }

        protected ICalcLabourHoursProxy CalcLabourHoursProxy { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.StockReportService = Substitute.For<IStockReportService>();

            this.CalcLabourHoursProxy = Substitute.For<ICalcLabourHoursProxy>();

            this.StockReportFacadeService = new StockReportFacadeService(
                this.StockReportService, new ReportReturnResourceBuilder(),
                this.CalcLabourHoursProxy);

            this.Client = TestClient.With<StockReportModule>(
                services =>
                {
                    services.AddSingleton(this.StockReportFacadeService);
                    services.AddHandlers();
                    services.AddRouting();
                });
        }
    }
}
