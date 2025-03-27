namespace Linn.Stores2.Domain.LinnApps.Tests.StockServiceTests
{
    using System.Collections.Generic;

    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Tests.Extensions;
    using Linn.Stores2.Persistence.LinnApps.Repositories;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IStockService Sut { get; set; }

        protected TestServiceDbContext DbContext { get; private set; }

        protected IRepository<StockLocator, int> StockLocatorRepository { get; private set; }

        protected int LocationId { get; set; }

        protected int PalletNumber { get; set; }

        protected string PartNumber { get; set; }

        protected IEnumerable<StockLocator> Results { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();

            this.StockLocatorRepository = new StockLocatorRepository(this.DbContext);
            this.LocationId = 123;
            this.PalletNumber = 456;
            this.PartNumber = "P1";

            this.DbContext.StockLocators.AddAndSave(
                this.DbContext,
                new StockLocator { Id = 1, LocationId = this.LocationId, PartNumber = this.PartNumber, State = "OK", Category = "C1", CurrentStock = "Y", Quantity = 100, QuantityAllocated = 0 });
            this.DbContext.StockLocators.AddAndSave(
                this.DbContext,
                new StockLocator { Id = 2, LocationId = this.LocationId, PartNumber = this.PartNumber, State = "OK", Category = "C1", CurrentStock = "Y" });
            this.DbContext.StockLocators.AddAndSave(
                this.DbContext,
                new StockLocator { Id = 3, PalletNumber = this.PalletNumber, PartNumber = this.PartNumber, State = "OK", Category = "C1", CurrentStock = "Y" });
            this.DbContext.StockLocators.AddAndSave(
                this.DbContext,
                new StockLocator { Id = 4, LocationId = this.LocationId, PartNumber = "P2", State = "OK", Category = "C1", CurrentStock = "Y" });
            this.DbContext.StockLocators.AddAndSave(
                this.DbContext,
                new StockLocator { Id = 5, PalletNumber = 900, PartNumber = "2STATES", State = "OK", Category = "C1", CurrentStock = "Y", Quantity = 100, QuantityAllocated = 0 });
            this.DbContext.StockLocators.AddAndSave(
                this.DbContext,
                new StockLocator { Id = 6, PalletNumber = 900, PartNumber = "2STATES", State = "QC", Category = "C1", CurrentStock = "Y", Quantity = 100, QuantityAllocated = 0  });
            this.DbContext.StockLocators.AddAndSave(
                this.DbContext,
                new StockLocator { Id = 7, PalletNumber = 1000, PartNumber = "INDEMAND", State = "OK", Category = "C1", CurrentStock = "Y", Quantity = 100, QuantityAllocated = 90  });

            this.Sut = new StockService(this.StockLocatorRepository);
        }
        
        [OneTimeTearDown]
        public void TearDownContext()
        {
            this.DbContext.Dispose();
        }
        
        [TearDown]
        public void Teardown()
        {
            this.DbContext.StockLocators.RemoveAllAndSave(this.DbContext);
        }
    }
}
