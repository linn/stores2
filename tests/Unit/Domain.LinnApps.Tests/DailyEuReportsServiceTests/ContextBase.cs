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

        protected IRepository<InterCompanyInvoice, InterCompanyInvoiceKey> InterCompanyInvoiceRepository { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.InterCompanyInvoiceRepository = Substitute.For<IRepository<InterCompanyInvoice, InterCompanyInvoiceKey>>();
            this.ReportingHelper = new ReportingHelper();

            this.Sut = new DailyEuReportsService(
                this.ReportingHelper,
                this.InterCompanyInvoiceRepository);
        }
    }
}
