namespace Linn.Stores2.Domain.LinnApps.Tests.QcLabelPrinterServiceTests
{
    using System.Collections.Generic;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    
    using Linn.Stores2.Domain.LinnApps.Labels;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenKardexLocationAndLines : ContextBase
    {
        private QcLabelPrintRequest request;
        
        private LabelType specifiedPrinterLabelType;
        
        private LabelType kardexLabelType;

        private PurchaseOrderResult purchaseOrderResult;

        private Part part;

        private LabelType defaultQcLabelType;
        
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
                                                     DefaultPrinter = "PRINTER DEFAULT", FileName = "TEMPLATE"
                                                 };
            
            this.defaultQcLabelType = new LabelType
            {
                DefaultPrinter = "PASS LABEL", FileName = "P TEMPLATE"
            };

            this.LabelTypeRepository.FindByAsync(Arg.Any<Expression<Func<LabelType, bool>>>())
                .Returns(this.kardexLabelType, this.specifiedPrinterLabelType, this.defaultQcLabelType, this.specifiedPrinterLabelType);

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
                KardexLocation = "K106",
                ReqNumber = 54321,
                Lines = new List<LabelLine>
                {
                    new LabelLine(1, 1)
                },
                PrinterName = "SPEC PRINTER"
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
            
            this.DocumentProxy.GetPurchaseOrder(request.OrderNumber).Returns(this.purchaseOrderResult);
            
            this.EmployeeRepository.FindByIdAsync(this.request.UserNumber)
                .Returns(new Employee { Name = "MR EMPLOYEE" });
            
            this.LabelPrinter.PrintLabelsAsync(
                    $"KGI{this.request.OrderNumber}",
                    this.specifiedPrinterLabelType.DefaultPrinter,
                    2,
                    this.kardexLabelType.FileName,
                    $"\"{this.request.KardexLocation.Replace("\"", "''")}\",\"{this.request.ReqNumber}\"")
                .Returns((true, string.Empty));
            this.LabelPrinter.PrintLabelsAsync(
                    $"QC {request.OrderNumber}-1",
                    this.specifiedPrinterLabelType.DefaultPrinter,
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
            await this.Sut.PrintLabels(this.request);
        }

        [Test]
        public void ShouldPrintBoth()
        {
            this.LabelPrinter.Received(1).PrintLabelsAsync(
                $"KGI{this.request.OrderNumber}",
                this.specifiedPrinterLabelType.DefaultPrinter,
                2,
                this.kardexLabelType.FileName,
                $"\"{this.request.KardexLocation.Replace("\"", "''")}\",\"{this.request.ReqNumber}\"");
            
                this.LabelPrinter.Received(1).PrintLabelsAsync(
                $"QC {request.OrderNumber}-1",
                this.specifiedPrinterLabelType.DefaultPrinter,
                this.request.Qty, 
                this.defaultQcLabelType.FileName,
                "\"12345\",\"PART\",\"1\",\"ME\",\"A PART\",\"54321\",\"MAY082025\",\"**ROHS Compliant**\"\n");
        }
    }
}
