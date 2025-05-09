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

    public class WhenKardexLocationAndPrinterNameSpecified : ContextBase
    {
        private ProcessResult result;

        private QcLabelPrintRequest request;

        private LabelType kardexLabelType;

        private LabelType specifiedPrinterLabelType;
        
        [SetUp]
        public async Task SetUp()
        {
            this.kardexLabelType = new LabelType
                                       {
                                           DefaultPrinter = "KARDEX DEFAULT", 
                                           FileName = "TEMPLATE"
                                       };

            this.specifiedPrinterLabelType = new LabelType
                                                 {
                                                     DefaultPrinter = "PRINTER DEFAULT", FileName = "P TEMPLATE"
                                                 };

            this.LabelTypeRepository.FindByAsync(Arg.Any<Expression<Func<LabelType, bool>>>())
                .Returns(this.kardexLabelType, this.specifiedPrinterLabelType);

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
                PrinterName = "SPEC PRINTER"
            };
            this.EmployeeRepository.FindByIdAsync(this.request.UserNumber)
                .Returns(new Employee { Name = "MR EMPLOYEE" });

            this.LabelPrinter.PrintLabelsAsync(
                $"KGI{this.request.OrderNumber}",
                this.specifiedPrinterLabelType.DefaultPrinter,
                this.request.Qty,
                this.kardexLabelType.FileName,
                $"\"{this.request.KardexLocation.Replace("\"", "''")}\",\"{this.request.ReqNumber}\"")
                .Returns((true, string.Empty));

            this.result = await this.Sut.PrintLabels(this.request);
        }

        [Test]
        public void ShouldPrintLabels()
        {
            this.LabelPrinter.Received().PrintLabelsAsync(
                $"KGI{this.request.OrderNumber}",
                this.specifiedPrinterLabelType.DefaultPrinter,
                this.request.Qty,
                this.kardexLabelType.FileName,
                $"\"{this.request.KardexLocation.Replace("\"", "''")}\",\"{this.request.ReqNumber}\"");
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Success.Should().BeTrue();
        }
    }
}


