namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingPartNumberChangeAndInvalidPartNumberChange : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.DepartmentRepository.FindByIdAsync("0000042808")
                .Returns(new Department("0000042808", "FINAL ASSEMBLY"));
            this.NominalRepository.FindByIdAsync("0000000480")
                .Returns(new Nominal("0000000480", "REWORK"));
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.PartNumberChange.FunctionCode)
                .Returns(TestFunctionCodes.PartNumberChange);
            this.PalletRepository.FindByIdAsync(365).Returns(new StoresPallet());
            this.PalletRepository.FindByIdAsync(1000).Returns(new StoresPallet());
            this.PartRepository.FindByIdAsync("ADIKT").Returns(new Part { PartNumber = "ADIKT", ProductAnalysisCode = "ADIKT" });
            this.PartRepository.FindByIdAsync("KEEL").Returns(new Part { PartNumber = "KEEL", ProductAnalysisCode = "LP12" });
            this.StateRepository.FindByIdAsync("STORES").Returns(new StockState("STORES", "MY LOVELY STOCK"));
            this.StockPoolRepository.FindByIdAsync("LOAN POOL").Returns(new StockPool());
            this.StockService.ValidStockLocation(null, 365, Arg.Any<string>(), Arg.Any<decimal>(), Arg.Any<string>())
                .Returns(new ProcessResult(true, null));
            this.StoresService.ValidOntoLocation(
                Arg.Any<Part>(),
                Arg.Any<StorageLocation>(),
                Arg.Any<StoresPallet>(),
                Arg.Any<StockState>()).Returns(new ProcessResult(true, null));
            this.StoresService.ValidPartNumberChange(Arg.Any<Part>(), Arg.Any<Part>())
                .Returns(new ProcessResult(false, "Invalid part number change"));
            this.StoresService.ValidNominalAccount("0000042808", "0000000480")
                .Returns(new NominalAccount());

            this.action = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.PartNumberChange.FunctionCode,
                null,
                null,
                null,
                "0000042808",
                "0000000480",
                fromStockPool: "LOAN POOL",
                toStockPool: "LOAN POOL",
                fromPalletNumber: 365,
                toPalletNumber: 1000,
                partNumber: "ADIKT",
                fromState: "STORES",
                toState: "STORES",
                batchRef: "Q3347702",
                batchDate: new DateTime(2024, 7, 22),
                newPartNumber: "KEEL");
        }

        [Test]
        public async Task ShouldThrowCorrectException()
        {
            await this.action.Should().ThrowAsync<RequisitionException>()
                .WithMessage("Invalid part number change");
        }
    }
}
