namespace Linn.Stores2.Domain.LinnApps.Tests.StoragePlaceAuditReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected StoragePlaceAuditReportService Sut { get; set; }

        protected IRepository<StockLocator, int> StockLocatorRepository { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        protected IQueryRepository<StoragePlace> StoragePlaceQueryRepository { get; private set; }

        protected IStoragePlaceAuditPack StoragePlaceAuditPack { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.StockLocatorRepository = Substitute.For<IRepository<StockLocator, int>>();
            this.ReportingHelper = new ReportingHelper();
            this.StoragePlaceQueryRepository = Substitute.For<IQueryRepository<StoragePlace>>();
            this.StoragePlaceAuditPack = Substitute.For<IStoragePlaceAuditPack>();
            this.Sut = new StoragePlaceAuditReportService(
                this.ReportingHelper,
                this.StockLocatorRepository,
                this.StoragePlaceAuditPack,
                this.StoragePlaceQueryRepository);
        }
    }
}
