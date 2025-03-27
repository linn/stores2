namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingWOff : ContextBase
    {
        private RequisitionHeader result;

        [SetUp]
        public async Task SetUp()
        {
            this.DepartmentRepository.FindByIdAsync("0000042808")
                .Returns(new Department("0000042808", "FINAL ASSEMBLY"));
            this.NominalRepository.FindByIdAsync("0000004729")
                .Returns(new Nominal("0000004729", "STOCK ADJUSTMENTS"));
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.WriteOff.FunctionCode)
                .Returns(TestFunctionCodes.WriteOff);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.WriteOff.TransactionCode)
                .Returns(TestTransDefs.WriteOff);
            this.PalletRepository.FindByIdAsync(123).Returns(new StoresPallet());
            this.PartRepository.FindByIdAsync("PART").Returns(new Part());
            this.ReqStoredProcedures.CanPutPartOnPallet("PART", 123).Returns(true);
            this.result = await this.Sut.Validate(
                33087,
                TestFunctionCodes.WriteOff.FunctionCode,
                "F",
                null,
                null,
                "0000042808",
                "0000004729",
                new LineCandidate
                {
                    PartNumber = "PART",
                    Qty = 1,
                    TransactionDefinition = TestTransDefs.WriteOff.TransactionCode,
                    Moves = new[] { new MoveSpecification { Qty = 1, ToPallet = 123 } }
                });
        }

        [Test]
        public void ShouldReturnValidated()
        {
            this.result.Should().NotBeNull();
        }
    }
}
