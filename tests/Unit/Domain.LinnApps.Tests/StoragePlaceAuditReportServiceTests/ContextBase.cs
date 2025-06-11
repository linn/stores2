namespace Linn.Stores2.Domain.LinnApps.Tests.StoragePlaceAuditReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected StoragePlaceAuditReportService Sut { get; set; }

        protected IRepository<StockLocator, int> StockLocatorRepository { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        protected IQueryRepository<StoragePlace> StoragePlaceQueryRepository { get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        protected IRepository<Department, string> DepartmentRepository { get; private set; }

        protected IRequisitionFactory RequisitionFactory { get; private set; }

        protected IRequisitionManager RequisitionManager { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.StockLocatorRepository = Substitute.For<IRepository<StockLocator, int>>();
            this.ReportingHelper = new ReportingHelper();
            this.StoragePlaceQueryRepository = Substitute.For<IQueryRepository<StoragePlace>>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.RequisitionFactory = Substitute.For<IRequisitionFactory>();
            this.RequisitionManager = Substitute.For<IRequisitionManager>();
            this.DepartmentRepository = Substitute.For<IRepository<Department, string>>();

            this.Sut = new StoragePlaceAuditReportService(
                this.ReportingHelper,
                this.StockLocatorRepository,
                this.StoragePlaceQueryRepository,
                this.RequisitionFactory,
                this.RequisitionManager,
                this.EmployeeRepository,
                this.DepartmentRepository);
        }
    }
}
