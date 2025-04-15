﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenValidatingAndMoveOntoSpecifiesNonExistingPallet : ContextBase
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
            var part = new Part { PartNumber = "PART" };
            this.PartRepository.FindByIdAsync(part.PartNumber).Returns(part);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.LinnDeptToStock.TransactionCode)
                .Returns(TestTransDefs.StockToLinnDept);
            this.action = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.LinnDeptReq.FunctionCode,
                "O",
                null,
                null,
                "1607",
                "2963",
                new LineCandidate
                    {
                        Qty = 1,
                        PartNumber = part.PartNumber,
                        TransactionDefinition = TestTransDefs.LinnDeptToStock.TransactionCode,
                        Moves = new[] { new MoveSpecification { Qty = 1, ToPallet = 666 } }
                    });
        }

        [Test]
        public async Task ShouldThrowException()
        {
            await this.action.Should()
                .ThrowAsync<InsertReqOntosException>().WithMessage("Pallet 666 is invalid");
        }
    }
}
