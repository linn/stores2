﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
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

    public class WhenValidatingPartNumberChangeAndPartsFromDifferentProductGroups : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public async Task SetUp()
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
            this.PartRepository.FindByIdAsync("ADIKT").Returns(new Part { PartNumber = "ADIKT", ProductAnalysisCode = "ADIKT"});
            this.PartRepository.FindByIdAsync("KEEL").Returns(new Part { PartNumber = "KEEL", ProductAnalysisCode = "LP12"});
            this.StateRepository.FindByIdAsync("STORES").Returns(new StockState("STORES", "MY LOVELY STOCK"));
            this.StockPoolRepository.FindByIdAsync("LOAN POOL").Returns(new StockPool());
            this.StockService.ValidStockLocation(null, 365, Arg.Any<string>(), Arg.Any<decimal>(), Arg.Any<string>())
                .Returns(new ProcessResult(true, null));
            this.StoresService.ValidOntoLocation(
                Arg.Any<Part>(),
                Arg.Any<StorageLocation>(),
                Arg.Any<StoresPallet>(),
                Arg.Any<StockState>()).Returns(new ProcessResult(true, null));
            this.action = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.PartNumberChange.FunctionCode,
                null,
                null,
                null,
                "0000042808",
                "0000000480",
                null,
                partNumber: "ADIKT",
                newPartNumber: "KEEL",
                fromState: "STORES",
                fromStockPool: "LOAN POOL",
                fromPalletNumber: 365,
                batchRef: "Q3347702",
                batchDate: new DateTime(2024, 7, 22),
                toState: "STORES",
                toStockPool: "LOAN POOL",
                toPalletNumber: 1000);
        }

        [Test]
        public async Task ShouldThrowCorrectException()
        {
            await this.action.Should().ThrowAsync<RequisitionException>()
                .WithMessage("Old part is for product group ADIKT new part is for product group LP12");
        }
    }
}
