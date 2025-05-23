namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingBookWoBomVerificationOverdue : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(123).Returns(new Employee { Id = 123 });
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.BookWorksOrder.FunctionCode)
                .Returns(TestFunctionCodes.BookWorksOrder);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.CustomerToGoodStock.TransactionCode)
                .Returns(TestTransDefs.CustomerToGoodStock);
            this.PalletRepository.FindByIdAsync(502).Returns(new StoresPallet());
            this.PalletRepository.FindByIdAsync(503).Returns(new StoresPallet());
            this.PartRepository.FindByIdAsync("PART").Returns(new Part { PartNumber = "PART", BomVerifyFreqWeeks = 2 });
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
            this.BomVerificationProxy.GetBomVerifications("PART")
                .Returns(new List<BomVerificationHistory> { new BomVerificationHistory { DateVerified = 1.April(1870) } });

            this.action = () => this.Sut.Validate(
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
                quantity: 1,
                toState: "STORES");
        }

        [Test]
        public async Task ShouldThrowCorrectException()
        {
            await this.action.Should().ThrowAsync<CreateRequisitionException>()
                .WithMessage("Part number PART was due for bom verification on 15-Apr-1870.");
        }
    }
}
