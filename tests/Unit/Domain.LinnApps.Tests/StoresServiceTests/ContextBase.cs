namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests
{
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Stores;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IStoresService Sut { get; set; }
        
        protected IStockService StockService { get; private set; }

        protected ProcessResult Result { get; set; }

        protected Part Part { get; set; }

        protected StorageLocation Location { get; set; }

        protected StoresPallet Pallet { get; set; }

        protected StockState OnToState { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.StockService = Substitute.For<IStockService>();
            this.Part = new Part { PartNumber = "P1" };
            this.Location = new StorageLocation { LocationId = 123 };
            this.Pallet = new StoresPallet();
            this.OnToState = new StockState();

            this.Sut = new StoresService(this.StockService);
        }
    }
}
