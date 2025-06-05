namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected RequisitionReportService Sut { get; set; }

        protected IRepository<RequisitionHeader, int> RequisitionRepository { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.RequisitionRepository = Substitute.For<IRepository<RequisitionHeader, int>>();
            this.ReportingHelper = new ReportingHelper();

            this.Sut = new RequisitionReportService(this.RequisitionRepository, this.ReportingHelper);
        }
    }
}
