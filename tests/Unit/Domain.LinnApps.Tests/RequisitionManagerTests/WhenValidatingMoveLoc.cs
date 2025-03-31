namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingMoveLoc : ContextBase
    {
        private RequisitionHeader result;

        [SetUp]
        public async Task SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.MoveLocation.FunctionCode)
                .Returns(TestFunctionCodes.MoveLocation);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.CustomerToGoodStock.TransactionCode)
                .Returns(TestTransDefs.CustomerToGoodStock);
            this.PalletRepository.FindByIdAsync(502).Returns(new StoresPallet());
            this.PalletRepository.FindByIdAsync(503).Returns(new StoresPallet());
            this.result = await this.Sut.Validate(
                33087,
                TestFunctionCodes.MoveLocation.FunctionCode,
                null,
                null,
                null,
                null,
                null,
                null,
                fromPalletNumber: 502,
                toPalletNumber: 503);
        }

        [Test]
        public void ShouldReturnValidated()
        {
            this.result.Should().NotBeNull();
        }
    }
}
