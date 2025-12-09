namespace Linn.Stores2.Domain.LinnApps.Tests.DailyEuReportsServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Reports;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected DailyEuReportsService Sut { get; set; }

        protected IRepository<DailyEuDespatchReport, int> DailyEuDespatchRepository { get; private set; }

        protected IRepository<DailyEuRsnImportReport, int> DailyEuRsnImportRepository { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DailyEuDespatchRepository = Substitute.For<IRepository<DailyEuDespatchReport, int>>();
            this.DailyEuRsnImportRepository = Substitute.For<IRepository<DailyEuRsnImportReport, int>>();
            this.ReportingHelper = new ReportingHelper();

            this.Sut = new DailyEuReportsService(
                this.ReportingHelper,
                this.DailyEuRsnImportRepository,
                this.DailyEuDespatchRepository);
        }
    }
}