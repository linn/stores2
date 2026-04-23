namespace Linn.Stores2.Domain.LinnApps.Tests.ImportReportServiceTests
{
    using Common.Rendering;

    using global::Linn.Common.Persistence;
    using global::Linn.Common.Reporting.Models;

    using Imports;
    using Imports.Models;

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

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.ReportingHelper = new ReportingHelper();
            this.ImportBookRepository = Substitute.For<IRepository<ImportBook, int>>();
            this.ImportMasterRepository = Substitute.For<ISingleRecordRepository<ImportMaster>>();
            this.ImportAuthNumberRepository = Substitute.For<IQueryRepository<ImportAuthNumber>>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();

            this.Sut = new ImportReportService(
                this.ImportBookRepository,
                this.ImportMasterRepository,
                this.ImportAuthNumberRepository,
                this.ClearanceHtmlTemplateService,
                this.EmployeeRepository,
                this.ReportingHelper);
        }
    }
}
