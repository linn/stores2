﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenValidatingAndNoLinesWhenRequiredByStoresFunction : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.DepartmentRepository.FindByIdAsync("1607")
                .Returns(new Department("1607", "DESC"));
            this.NominalRepository.FindByIdAsync("2963")
                .Returns(new Nominal("2963", "DESC"));
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.LinnDeptReq.FunctionCode)
                .Returns(TestFunctionCodes.LinnDeptReq);
            this.PalletRepository.FindByIdAsync(666).Returns(new StoresPallet());
            this.ReqStoredProcedures.CanPutPartOnPallet("PART", 666).Returns(true);
            this.PartRepository.FindByIdAsync("PART").Returns(new Part());
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StockToLinnDept.TransactionCode)
                .Returns(TestTransDefs.StockToLinnDept);

            this.action = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.LinnDeptReq.FunctionCode,
                "F",
                null,
                null,
                "1607",
                "2963",
                null);
        }

        [Test]
        public async Task ShouldThrowException()
        {
            await this.action.Should()
                .ThrowAsync<CreateRequisitionException>().WithMessage(
                    $"Lines are required for {TestFunctionCodes.LinnDeptReq.FunctionCode}");
        }
    }
}
