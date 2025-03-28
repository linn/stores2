namespace Linn.Stores2.Domain.LinnApps.Tests.StoresTransViewerTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected StoresTransViewerReportService Sut { get; set; }

        protected IRepository<StockTransaction, int> StockTransactionRepository { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.StockTransactionRepository = Substitute.For<IRepository<StockTransaction, int>>();
            this.ReportingHelper = new ReportingHelper();
            this.Sut = new StoresTransViewerReportService(
                this.ReportingHelper,
                this.StockTransactionRepository);
        }
    }
}
