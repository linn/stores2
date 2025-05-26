namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingAndCreditNotePartSourceButOverbookingLine : ContextBase
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
            this.DocumentProxy.GetCreditNote(1234, 1)
                .Returns(new DocumentResult("C", 1234, 1, 9, TestParts.SelektHub.PartNumber));

            var requisitions = new List<RequisitionHeader>
            {
                new RequisitionHeader(
                    new Employee(),
                    TestFunctionCodes.CustomerReturn,
                    null,
                    1,
                    "C",
                    null,
                    null,
                    part: TestParts.SelektHub,
                    quantity: 6,
                    document1Line: 1,
                    toStockPool: "LINN",
                    toState: "STORES"),
                new RequisitionHeader(
                    new Employee(),
                    TestFunctionCodes.CustomerReturn,
                    null,
                    1,
                    "C",
                    null,
                    null,
                    part: TestParts.SelektHub,
                    quantity: 3,
                    document1Line: 1,
                    toStockPool: "LINN",
                    toState: "STORES")
            };
            this.ReqRepository.FilterByAsync(Arg.Any<Expression<Func<RequisitionHeader, bool>>>())
                .Returns(requisitions);

            this.act = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.CustomerReturn.FunctionCode,
                null,
                1234,
                "C",
                null, 
                null,
                toStockPool: "LINN",
                toPalletNumber: 666,
                quantity: 1,
                toState: "STORES",
                document1Line: 1);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.act.Should().ThrowAsync<DocumentException>().WithMessage("Trying to overbook this line");
        }
    }
}
