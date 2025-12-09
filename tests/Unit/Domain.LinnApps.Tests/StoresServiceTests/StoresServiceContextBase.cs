namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests
{
    using Linn.Common.Domain;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Stores;

    using NSubstitute;

    using NUnit.Framework;

    public class StoresServiceContextBase
    {
        protected IStoresService Sut { get; set; }
        
        protected IStockService StockService { get; private set; }

        protected ProcessResult Result { get; set; }

        protected Part Part { get; set; }

        protected StorageLocation Location { get; set; }

        protected StoresPallet Pallet { get; set; }

        protected StockState OnToState { get; set; }

        protected IRepository<StoresTransactionState, StoresTransactionStateKey> StoresTransactionStateRepository { get; private set; }
        
        protected IRepository<StoresBudget, int> StoresBudgetRepository { get; private set; }

        protected IRepository<StockLocator, int> StockLocatorRepository { get; private set; }

        protected IRepository<RequisitionHeader, int> RequisitionRepository { get; private set; }

        protected IRepository<NominalAccount, int> NominalAccountRepository { get; private set; }

        protected IRepository<PartStorageType, int> PartStorageTypeRepository { get; private set; }

        protected IRepository<StorageLocation, int> StorageLocationRepository { get; private set; }

        protected IRepository<StoresPallet, int> PalletRepository { get; private set; }

        protected IRepository<StockPool, string> StockPoolRepository { get; private set; }

        [SetUp]
        public void SetUpServiceContext()
        {
            this.StockService = Substitute.For<IStockService>();
            this.Part = new Part { PartNumber = "P1" };
            this.Location = new StorageLocation { LocationId = 123, LocationCode = "LOC" };
            this.Pallet = new StoresPallet();
            this.OnToState = new StockState();
            this.StoresTransactionStateRepository = Substitute.For<IRepository<StoresTransactionState, StoresTransactionStateKey>>();
            this.StoresBudgetRepository = Substitute.For<IRepository<StoresBudget, int>>();
            this.StockLocatorRepository = Substitute.For<IRepository<StockLocator, int>>();
            this.RequisitionRepository = Substitute.For<IRepository<RequisitionHeader, int>>();
            this.NominalAccountRepository = Substitute.For<IRepository<NominalAccount, int>>();
            this.PartStorageTypeRepository = Substitute.For<IRepository<PartStorageType, int>>();
            this.StorageLocationRepository = Substitute.For<IRepository<StorageLocation, int>>();
            this.PalletRepository = Substitute.For<IRepository<StoresPallet, int>>();
            this.StockPoolRepository = Substitute.For<IRepository<StockPool, string>>();

            this.Sut = new StoresService(
                this.StockService, 
                this.StoresTransactionStateRepository, 
                this.StoresBudgetRepository,
                this.StockLocatorRepository,
                this.RequisitionRepository,
                this.NominalAccountRepository,
                this.PartStorageTypeRepository,
                this.StorageLocationRepository,
                this.PalletRepository,
                this.StockPoolRepository);
        }
    }
}
