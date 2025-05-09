namespace Linn.Stores2.Domain.LinnApps.Tests.QcLabelPrinterServiceTests
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Labels;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenKardexLocationAndNoPrinterNameSpecified : ContextBase
    {
        private ProcessResult result;

        private QcLabelPrintRequest request;

        private LabelType labelType;

        [SetUp]
        public async Task SetUp()
        {
            this.labelType = new LabelType { DefaultPrinter = "KARDEX DEFAULT", FileName = "TEMPLATE" };
            this.LabelTypeRepository.FindByAsync(Arg.Any<Expression<Func<LabelType, bool>>>())
                .Returns(this.labelType);

            this.request = new QcLabelPrintRequest
                               {
                                   DocType = "PO",
                                   PartNumber = "PART",
                                   DeliveryRef = "REF",
                                   Qty = 1,
                                   UserNumber = 33087,
                                   OrderNumber = 12345,
                                   NumberOfLines = 0,
                                   QcState = null,
                                   ReqNumber = 54321,
                                   KardexLocation = "K106",
                                   Lines = null,
                                   PrinterName = null
                               };
            this.EmployeeRepository.FindByIdAsync(this.request.UserNumber)
                .Returns(new Employee { Name = "MR EMPLOYEE" });

            this.LabelPrinter.PrintLabelsAsync(
                $"KGI{this.request.OrderNumber}",
                this.labelType.DefaultPrinter,
                this.request.Qty,
                this.labelType.FileName,
                $"\"{this.request.KardexLocation.Replace("\"", "''")}\",\"{this.request.ReqNumber}\"")
                .Returns((true, string.Empty));

            this.result = await this.Sut.PrintLabels(this.request);
        }

        [Test]
        public void ShouldPrintLabels()
        {
            this.LabelPrinter.Received().PrintLabelsAsync(
                $"KGI{this.request.OrderNumber}",
                this.labelType.DefaultPrinter,
                this.request.Qty,
                this.labelType.FileName,
                $"\"{this.request.KardexLocation.Replace("\"", "''")}\",\"{this.request.ReqNumber}\"");
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Success.Should().BeTrue();
        }
    }
}
