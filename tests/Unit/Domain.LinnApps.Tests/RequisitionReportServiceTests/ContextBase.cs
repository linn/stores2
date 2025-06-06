namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Rendering;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected RequisitionReportService Sut { get; set; }

        protected IRepository<RequisitionHeader, int> RequisitionRepository { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        protected IHtmlTemplateService<RequisitionHeader> HtmlTemplateService { get; private set; }
        
        [SetUp]
        public void SetUpContext()
        {
            this.RequisitionRepository = Substitute.For<IRepository<RequisitionHeader, int>>();
            this.ReportingHelper = new ReportingHelper();
            this.HtmlTemplateService = Substitute.For<IHtmlTemplateService<RequisitionHeader>>();

            this.Sut = new RequisitionReportService(
                this.RequisitionRepository,
                this.ReportingHelper,
                this.HtmlTemplateService);
        }
    }
}
