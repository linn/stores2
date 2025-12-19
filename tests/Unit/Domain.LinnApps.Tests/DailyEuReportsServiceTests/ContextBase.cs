namespace Linn.Stores2.Domain.LinnApps.Tests.DailyEuReportsServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Logistics;
    using Linn.Stores2.Domain.LinnApps.Reports;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected DailyEuReportsService Sut { get; set; }

        protected IQueryRepository<DailyEuDespatchReport> DailyEuDespatchRepository { get; private set; }

        protected IQueryRepository<DailyEuRsnImportReport> DailyEuRsnImportRepository { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        protected IFinanceProxy FinanceProxy { get; private set; }

        protected IRepository<ImportBookExchangeRate, ImportBookExchangeRateKey> ImportBookExchangeRateRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DailyEuDespatchRepository = Substitute.For<IQueryRepository<DailyEuDespatchReport>>();
            this.DailyEuRsnImportRepository = Substitute.For<IQueryRepository<DailyEuRsnImportReport>>();
            this.ReportingHelper = new ReportingHelper();
            this.FinanceProxy = Substitute.For<IFinanceProxy>();
            this.ImportBookExchangeRateRepository = Substitute.For<IRepository<ImportBookExchangeRate, ImportBookExchangeRateKey>>();

            this.Sut = new DailyEuReportsService(
                this.ReportingHelper,
                this.DailyEuRsnImportRepository,
                this.DailyEuDespatchRepository,
                this.FinanceProxy,
                this.ImportBookExchangeRateRepository);
        }
    }
}
