namespace Linn.Stores2.Domain.LinnApps.Tests.QcLabelPrinterServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Labels;
    using Linn.Stores2.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenLinesAndQcStateIsPassAndNoPrinterSpecified : ContextBase
    {
        private ProcessResult result;

        private QcLabelPrintRequest request;
        
        private LabelType specifiedPrinterLabelType;
        
        private PurchaseOrderResult purchaseOrderResult;

        private Part part;

        private LabelType defaultQcLabelType;
        
        [SetUp]
        public async Task SetUp()
        {
            this.specifiedPrinterLabelType = new LabelType
                                                 {
                                                     DefaultPrinter = "PRINTER DEFAULT", FileName = "TEMPLATE"
                                                 };
            
            this.defaultQcLabelType = new LabelType
            {
                DefaultPrinter = "PASS LABEL", FileName = "P TEMPLATE"
            };

            this.LabelTypeRepository.FindByAsync(Arg.Any<Expression<Func<LabelType, bool>>>())
                .Returns(this.defaultQcLabelType, this.specifiedPrinterLabelType);

            this.request = new QcLabelPrintRequest
            {
                DocType = "PO",
                PartNumber = "PART",
                DeliveryRef = "REF",
                Qty = 1,
                UserNumber = 33087,
                OrderNumber = 12345,
                NumberOfLines = 0,
                QcState = "PASS",
                ReqNumber = 54321,
                Lines = new List<LabelLine>
                {
                    new LabelLine(1, 1)
                }
            };

            this.purchaseOrderResult = new PurchaseOrderResult
            {
                OrderNumber = this.request.OrderNumber,
                SupplierId = 666,
                SupplierName = "HELL INC.",
                Details = new List<PurchaseOrderDetailResult>
                {
                    new()
                    {
                        PartNumber = this.request.PartNumber,
                        RohsCompliant = "Y"
                    }
                }
            };
            
            this.DocumentProxy.GetPurchaseOrder(this.request.OrderNumber).Returns(this.purchaseOrderResult);
            
            this.EmployeeRepository.FindByIdAsync(this.request.UserNumber)
                .Returns(new Employee { Name = "MR EMPLOYEE" });

            this.LabelPrinter.PrintLabelsAsync(
                    $"QC {request.OrderNumber}-1",
                    this.defaultQcLabelType.DefaultPrinter,
                    this.request.Qty,
                    this.defaultQcLabelType.FileName,
                    Arg.Any<string>())
                .Returns((true, string.Empty));
    
            this.part = new Part
            {
                PartNumber = this.request.PartNumber,
                Description = "A PART",
                OurUnitOfMeasure = "ONES"
            };
            
            this.PartsRepository.FindByAsync(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(this.part);
            this.result = await this.Sut.PrintLabels(this.request);
        }

        [Test]
        public void ShouldPrintLabels()
        {
            var today = DateTime.Today.ToString("MMMddyyyy").ToUpper();

            this.LabelPrinter.Received().PrintLabelsAsync(
                $"QC {request.OrderNumber}-1",
                this.defaultQcLabelType.DefaultPrinter,
                this.request.Qty,
                this.defaultQcLabelType.FileName,
                $"\"12345\",\"PART\",\"1\",\"ME\",\"A PART\",\"54321\",\"{today}\",\"**ROHS Compliant**\"{Environment.NewLine}");
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Success.Should().BeTrue();
        }
    }
}
