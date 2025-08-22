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
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenValidatingAndHeaderSpecifiesNonExistentToState : ContextBase
    {
        private Func<Task> act;

        [SetUp]
        public void SetUp()
        {
            this.DepartmentRepository.FindByIdAsync("1607")
                .Returns(new Department("1607", "DESC"));
            this.NominalRepository.FindByIdAsync("2963")
                .Returns(new Nominal("2963", "DESC"));
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.Move.FunctionCode)
                .Returns(TestFunctionCodes.Move);
            this.ReqStoredProcedures.CanPutPartOnPallet("PART", 666).Returns(true);
            this.PartRepository.FindByIdAsync("PART").Returns(new Part());
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StoresMove.TransactionCode)
                .Returns(TestTransDefs.StoresMove);
            this.PalletRepository.FindByIdAsync(666).Returns(new StoresPallet());
            this.StoresService.ValidNominalAccount("1607", "2963")
                .Returns(new NominalAccount());

            this.act = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.Move.FunctionCode,
                "F",
                null,
                null,
                "1607",
                "2963",
                toStockPool: "LINN",
                toPalletNumber: 666,
                partNumber: "PART",
                toState: "BLAH");
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.act.Should().ThrowAsync<RequisitionException>().WithMessage("To state BLAH does not exist");
        }
    }
}
