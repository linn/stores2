namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.DefaultStockPoolTests
{
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase : StoresServiceContextBase
    {
        public StorageLocation LocationWithDefaultStockPool { get; set; }

        public StoresPallet PalletWithDefaultStockPool { get; set; }

        public StoresPallet PalletWithNoDefaultStockPool { get; set; }

        public StockPool StockPool { get; set; }

        public StockPool DefaultStockPool { get; set; }

        public StockPool StockPoolResult { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.StockPool = new StockPool { StockPoolCode = "STOCK", BridgeId = 4935789 };
            this.DefaultStockPool = new StockPool { StockPoolCode = "LINN", BridgeId = 2 };

            this.LocationWithDefaultStockPool = new StorageLocation
                                                    {
                                                        LocationId = 123,
                                                        LocationCode = "E-PLACE",
                                                        DefaultStockPool = "STOCK"
                                                    };
            this.PalletWithDefaultStockPool = new StoresPallet { PalletNumber = 456, DefaultStockPool = this.StockPool };
            this.PalletWithNoDefaultStockPool = new StoresPallet { PalletNumber = 789 };
            
            this.PalletRepository.FindByIdAsync(this.PalletWithDefaultStockPool.PalletNumber)
                .Returns(this.PalletWithDefaultStockPool);
            this.PalletRepository.FindByIdAsync(this.PalletWithNoDefaultStockPool.PalletNumber)
                .Returns(this.PalletWithNoDefaultStockPool);
            this.StorageLocationRepository.FindByIdAsync(this.LocationWithDefaultStockPool.LocationId)
                .Returns(this.LocationWithDefaultStockPool);

            this.StockPoolRepository.FindByIdAsync("LINN").Returns(this.DefaultStockPool);
            this.StockPoolRepository.FindByIdAsync("STOCK").Returns(this.StockPool);
        }
    }
}
