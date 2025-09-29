namespace Linn.Stores2.Domain.LinnApps.Tests.StockReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using NSubstitute;
    using NUnit.Framework;

    public class ContextBase
    {
        protected StockReportService Sut { get; set; }

        protected IQueryRepository<TqmsData> TqmsRepository { get; private set; }

        protected IQueryRepository<LabourHoursSummary> LabourHoursSummaryRepository { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        protected IRepository<StockLocator, int> StockLocatorRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.TqmsRepository = Substitute.For<IQueryRepository<TqmsData>>();
            this.LabourHoursSummaryRepository = Substitute.For<IQueryRepository<LabourHoursSummary>>();
            this.ReportingHelper = new ReportingHelper();
            this.StockLocatorRepository = Substitute.For<IRepository<StockLocator, int>>();

            this.Sut = new StockReportService(
                this.TqmsRepository,
                this.LabourHoursSummaryRepository,
                this.ReportingHelper,
                this.StockLocatorRepository);
        }
    }
}
