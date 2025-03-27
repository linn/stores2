namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingAndCreditNotePartSourceButNoDocumentType : ContextBase
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
            this.DocumentProxy.GetCreditNote(1234, null)
                .Returns(new DocumentResult("C", 1234, null, null, null));
            this.act = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.CustomerReturn.FunctionCode,
                null,
                1234,
                null,
                null,
                null,
                null,
                toStockPool: "STORES",
                toState: "LINN",
                toPalletNumber: 666);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.act.Should().ThrowAsync<CreateRequisitionException>().WithMessage("Function requires a credit note");
        }
    }
}
