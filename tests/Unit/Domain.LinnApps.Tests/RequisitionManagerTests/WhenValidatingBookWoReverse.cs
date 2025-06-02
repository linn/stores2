namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingBookWoReverse : ContextBase
    {
        private RequisitionHeader result;

        [SetUp]
        public async Task SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(123).Returns(new Employee { Id = 123 });
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.BookWorksOrder.FunctionCode)
                .Returns(TestFunctionCodes.BookWorksOrder);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.CustomerToGoodStock.TransactionCode)
                .Returns(TestTransDefs.CustomerToGoodStock);
            this.PalletRepository.FindByIdAsync(502).Returns(new StoresPallet());
            this.PalletRepository.FindByIdAsync(503).Returns(new StoresPallet());
            this.PartRepository.FindByIdAsync("PART").Returns(new Part { PartNumber = "PART" });
            this.StateRepository.FindByIdAsync("STORES").Returns(new StockState("STORES", "LOVELY STOCK"));
            this.StockPoolRepository.FindByIdAsync("LINN").Returns(new StockPool());
            this.StockService.ValidStockLocation(null, 502, "PART", Arg.Any<decimal>(), Arg.Any<string>())
                .Returns(new ProcessResult(true, null));
            this.DocumentProxy.GetWorksOrder(123).Returns(
                new WorksOrderResult
                    {
                        PartNumber = "PART",
                        Quantity = 12,
                        QuantityBuilt = 1,
                        DateCancelled = null,
                        OrderNumber = 123,
                        Outstanding = "Y",
                        WorkStationCode = "WS1"
                    });
            this.SalesProxy.GetSalesArticle("PART").Returns(new SalesArticleResult { TypeOfSerialNumber = "N" });
            this.StoresService.ValidOntoLocation(
                Arg.Any<Part>(),
                Arg.Any<StorageLocation>(),
                Arg.Any<StoresPallet>(),
                Arg.Any<StockState>()).Returns(new ProcessResult(true, null));
            this.StoresService.ValidReverseQuantity(Arg.Any<int>(), -4)
                .Returns(new ProcessResult(true, "ok"));
            var toBeReversed = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.BookWorksOrder,
                null,
                123,
                "WO",
                null,
                null,
                reference: null,
                comments: "Uno reverse",
                quantity: 4);
            this.ReqRepository.FindByIdAsync(456).Returns(toBeReversed);
            this.result = await this.Sut.Validate(
                123,
                TestFunctionCodes.BookWorksOrder.FunctionCode,
                null,
                123,
                "WO",
                null,
                null,
                toStockPool: "LINN",
                fromPalletNumber: 502,
                toPalletNumber: 503,
                partNumber: "PART",
                quantity: -4,
                toState: "STORES",
                isReverseTransaction: "Y",
                originalDocumentNumber: 456);
        }

        [Test]
        public void ShouldGetWorksOrder()
        {
            this.DocumentProxy.Received().GetWorksOrder(123);
        }

        [Test]
        public void ShouldGetSalesArticle()
        {
            this.SalesProxy.Received().GetSalesArticle("PART");
        }

        [Test]
        public void ShouldCheckReverseQuantity()
        {
            this.StoresService.Received().ValidReverseQuantity(Arg.Any<int>(), -4);
        }

        [Test]
        public void ShouldReturnValidated()
        {
            this.result.Should().NotBeNull();
        }
    }
}
