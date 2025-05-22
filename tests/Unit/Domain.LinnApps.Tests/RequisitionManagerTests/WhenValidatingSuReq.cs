namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingSuReq : ContextBase
    {
        private RequisitionHeader result;

        [SetUp]
        public async Task SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.StockToSupplier.FunctionCode)
                .Returns(TestFunctionCodes.StockToSupplier);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StockToSupplier.TransactionCode)
                .Returns(TestTransDefs.StockToSupplier);
            this.PalletRepository.FindByIdAsync(123).Returns(new StoresPallet());
            this.PartRepository.FindByIdAsync("PART").Returns(new Part());
            this.ReqStoredProcedures.CanPutPartOnPallet("PART", 123).Returns(true);
            this.result = await this.Sut.Validate(
                33087,
                TestFunctionCodes.StockToSupplier.FunctionCode,
                null,
                null,
                null,
                null,
                null,
                toPalletNumber: 665,
                partNumber: "PART",
                lines: new List<LineCandidate> 
                           { 
                               new LineCandidate
                                   {
                                        PartNumber = "PART",
                                        Qty = 10m,
                                        TransactionDefinition = TestTransDefs.StockToSupplier.TransactionCode,
                                        Moves = new List<MoveSpecification>()
                                                    {
                                                        new MoveSpecification
                                                            {
                                                                Qty = 10,
                                                                ToPallet = 123
                                                            }
                                                    }
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
