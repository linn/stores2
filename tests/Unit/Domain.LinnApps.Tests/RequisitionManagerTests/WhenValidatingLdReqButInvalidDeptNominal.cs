namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;
    using NSubstitute.ExceptionExtensions;

    using NUnit.Framework;

    public class WhenValidatingLdReqButInvalidDeptNominal : ContextBase
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
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.LinnDeptToStock.TransactionCode)
                .Returns(TestTransDefs.LinnDeptToStock);
            this.PalletRepository.FindByIdAsync(123).Returns(new StoresPallet());
            this.PartRepository.FindByIdAsync("PART").Returns(new Part());
            this.ReqStoredProcedures.CanPutPartOnPallet("PART", 123).Returns(true);
            this.StoresService.ValidNominalAccount("1607", "2963")
                .Throws(new InvalidNominalAccountException("Dept Nom Not Ok"));

            this.action = () => this.Sut.Validate(
                                    33087,
                                    TestFunctionCodes.LinnDeptReq.FunctionCode,
                                    "O",
                                    null,
                                    null,
                                    "1607",
                                    "2963",
                                    lines: new List<LineCandidate>
                                               {
                                                   new LineCandidate
                                                       {
                                                           PartNumber = "PART",
                                                           Qty = 1,
                                                           TransactionDefinition =
                                                               TestTransDefs.LinnDeptToStock.TransactionCode,
                                                           Moves = new[]
                                                                       {
                                                                           new MoveSpecification
                                                                               {
                                                                                   Qty = 1, ToPallet = 123
                                                                               }
                                                                       }
                                                       }
                                               });
        }

        [Test]
        public async Task ShouldThrowCorrectException()
        {
            await this.action.Should().ThrowAsync<InvalidNominalAccountException>()
                .WithMessage("Dept Nom Not Ok");
        }
    }
}
