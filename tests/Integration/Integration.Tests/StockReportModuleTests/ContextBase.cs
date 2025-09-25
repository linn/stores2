using Linn.Common.Pdf;
using Linn.Common.Rendering;

namespace Linn.Stores2.Integration.Tests.StockReportModuleTests
{
    using System.Net.Http;

    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Models;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IStockReportFacadeService StockReportFacadeService { get; private set; }

        protected IStockReportService StockReportService { get; private set; }

        protected ICalcLabourHoursProxy CalcLabourHoursProxy { get; set; }

        protected IPdfService PdfService { get; set; }

        protected IHtmlTemplateService<LabourHoursInStockReport> htmlTemplateForLabourHoursInStock { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.StockReportService = Substitute.For<IStockReportService>();

            this.CalcLabourHoursProxy = Substitute.For<ICalcLabourHoursProxy>();

            this.PdfService = Substitute.For<IPdfService>();

            this.htmlTemplateForLabourHoursInStock = Substitute.For<IHtmlTemplateService<LabourHoursInStockReport>>();

            this.StockReportFacadeService = new StockReportFacadeService(
                this.StockReportService,
                new ReportReturnResourceBuilder(),
                this.CalcLabourHoursProxy,
                this.PdfService,
                this.htmlTemplateForLabourHoursInStock);

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
