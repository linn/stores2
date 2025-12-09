namespace Linn.Stores2.Domain.LinnApps.Tests.StorageTypeServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;
    using NUnit.Framework;

    public class ContextBase
    {
        protected StorageTypesService Sut { get; set; }

        protected IRepository<PartStorageType, int> PartStorageTypeRepository { get; private set; }

        protected IRepository<PcasStorageType, PcasStorageTypeKey> PcasStorageTypeRepository { get; private set; }

        protected IRepository<Part, string> PartsRepository { get; private set; }

        protected IRepository<StorageType, string> StorageTypeRepository { get; private set; }

        protected IRepository<PcasBoard, string> PcasBoardRepository { get; private set; }

        protected ITransactionManager TransactionManager { get; private set; }

        protected IRepository<StockLocator, int> StockLocatorRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {

            this.PartStorageTypeRepository = Substitute.For<IRepository<PartStorageType, int>>();
            this.PcasStorageTypeRepository = Substitute.For<IRepository<PcasStorageType, PcasStorageTypeKey>>();
            this.PartsRepository = Substitute.For<IRepository<Part, string>>();
            this.PcasBoardRepository = Substitute.For<IRepository<PcasBoard, string>>();
            this.StorageTypeRepository = Substitute.For<IRepository<StorageType, string>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.StockLocatorRepository = Substitute.For<IRepository<StockLocator, int>>();

            this.Sut = new StorageTypesService(
                this.PartStorageTypeRepository,
                this.PcasStorageTypeRepository,
                this.TransactionManager,
                this.PartsRepository,
                this.StorageTypeRepository,
                this.PcasBoardRepository);
        }
    }
}

