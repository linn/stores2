namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingAndCreditNotePartSourceAndCreditNoteNotExist : ContextBase
    {
        private Func<Task> act;

        [SetUp]
        public void SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.CustomerReturn.FunctionCode)
                .Returns(TestFunctionCodes.CustomerReturn);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.CustomerToGoodStock.TransactionCode)
                .Returns(TestTransDefs.CustomerToGoodStock);
            this.PalletRepository.FindByIdAsync(666).Returns(new StoresPallet { DateInvalid = DateTime.Today });
            this.act = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.CustomerReturn.FunctionCode,
                null,
                1234,
                "C",
                null,
                null,
                null,
                toStockPool: "STORES",
                toState: "STORES",
                toPalletNumber: 666);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.act.Should().ThrowAsync<DocumentException>().WithMessage("Could not find credit note 1234");
        }
    }
}
