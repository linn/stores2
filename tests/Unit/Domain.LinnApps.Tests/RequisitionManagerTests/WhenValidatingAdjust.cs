namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingAdjust : ContextBase
    {
        private RequisitionHeader result;

        [SetUp]
        public async Task SetUp()
        {
            this.DepartmentRepository.FindByIdAsync("0000042808")
                .Returns(new Department("0000042808", "FINAL ASSEMBLY"));
            this.NominalRepository.FindByIdAsync("0000004710")
                .Returns(new Nominal("0000004710", "STOCK ADJUSTMENTS"));
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.Adjust.FunctionCode)
                .Returns(TestFunctionCodes.Adjust);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StockToAdjust.TransactionCode)
                .Returns(TestTransDefs.StockToAdjust);
            this.PalletRepository.FindByIdAsync(123).Returns(new StoresPallet());
            this.PartRepository.FindByIdAsync("PART").Returns(new Part());
            this.ReqStoredProcedures.CanPutPartOnPallet("PART", 123).Returns(true);
            this.StoresService.ValidNominalAccount("0000042808", "0000004710")
                .Returns(new NominalAccount());
            this.result = await this.Sut.Validate(
                33087,
                TestFunctionCodes.Adjust.FunctionCode,
                "F",
                null,
                null,
                "0000042808",
                "0000004710",
                lines: new List<LineCandidate>
                           {
                               new LineCandidate
                                   {
                                       PartNumber = "PART",
                                       Qty = 1,
                                       TransactionDefinition = TestTransDefs.StockToAdjust.TransactionCode,
                                       Moves = new[] { new MoveSpecification { Qty = 1, ToPallet = 123 } }
                                   }
                           });
        }

        [Test]
        public void ShouldReturnValidated()
        {
            this.result.Should().NotBeNull();
        }
    }
}
