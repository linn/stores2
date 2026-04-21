namespace Linn.Stores2.Domain.LinnApps.Tests.ImportReportServiceTests
{
    using Common.Rendering;

    using global::Linn.Common.Persistence;
    using global::Linn.Common.Reporting.Models;
    using global::Linn.Stores2.Domain.LinnApps.External;
    using global::Linn.Stores2.Domain.LinnApps.Logistics;
    using global::Linn.Stores2.Domain.LinnApps.Reports;

    using Imports;
    using Imports.Models;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Logistics;
    using Linn.Stores2.Domain.LinnApps.Reports;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected ImportReportService Sut { get; set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        protected IRepository<ImportBook, int> ImportBookRepository { get; private set; }

        protected ISingleRecordRepository<ImportMaster> ImportMasterRepository { get; private set; }

        protected IQueryRepository<ImportAuthNumber> ImportAuthNumberRepository { get; private set; }

        protected IHtmlTemplateService<ImportClearanceInstruction> ClearanceHtmlTemplateService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.ReportingHelper = new ReportingHelper();
            this.ImportBookRepository = Substitute.For<IRepository<ImportBook, int>>();
            this.ImportMasterRepository = Substitute.For<ISingleRecordRepository<ImportMaster>>();
            this.ImportAuthNumberRepository = Substitute.For<IQueryRepository<ImportAuthNumber>>();

            this.Sut = new ImportReportService(
                this.ImportBookRepository,
                this.ImportMasterRepository,
                this.ImportAuthNumberRepository,
                this.ClearanceHtmlTemplateService,
                this.ReportingHelper);
        }
    }
}
