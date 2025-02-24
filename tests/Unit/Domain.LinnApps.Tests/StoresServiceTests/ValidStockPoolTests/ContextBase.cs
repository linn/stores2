namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidStockPoolTests
{
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NUnit.Framework;

    public class ContextBase : StoresServiceContextBase
    {
        public StockPool StockPool { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.StockPool = new StockPool { StockPoolCode = "SP1", AccountingCompanyCode = "AC1" };
        }
    }
}
