namespace Linn.Stores2.Domain.LinnApps.Tests.GoodsInLogTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.GoodsIn;
    using Linn.Stores2.Domain.LinnApps.Reports;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected GoodsInLogReportService Sut { get; set; }

        protected IRepository<GoodsInLogEntry, int> GoodsInLogRepository { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.GoodsInLogRepository = Substitute.For<IRepository<GoodsInLogEntry, int>>();
            this.ReportingHelper = new ReportingHelper();
            this.Sut = new GoodsInLogReportService(
                this.ReportingHelper,
                this.GoodsInLogRepository);
        }
    }
}
