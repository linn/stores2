namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenValidatingAndPartSourceIsPurchaseOrderButOrderNotAuthorised : ContextBase
    {
        private Func<Task> act;

        [SetUp]
        public void SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.GistPo.FunctionCode)
                .Returns(TestFunctionCodes.GistPo);
            this.ReqStoredProcedures.CanPutPartOnPallet("PART", 666).Returns(true);
            this.PartRepository.FindByIdAsync("PART").Returns(new Part());
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.InspectionToStores.TransactionCode)
                .Returns(TestTransDefs.InspectionToStores);
            this.PalletRepository.FindByIdAsync(666).Returns(new StoresPallet { DateInvalid = DateTime.Today });
            this.DocumentProxy.GetPurchaseOrder(1234567).Returns(new PurchaseOrderResult { IsAuthorised = false, DocumentType = "PO" });
            this.act = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.GistPo.FunctionCode,
                null,
                1234567,
                null,
                null,
                null,
                fromStockPool: "QC",
                toStockPool: "STORES",
                toPalletNumber: 666,
                partNumber: "PART",
                quantity: 10);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.act.Should().ThrowAsync<CreateRequisitionException>().WithMessage("PO 1234567 is not authorised!");
        }
    }
}
