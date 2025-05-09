namespace Linn.Stores2.Domain.LinnApps.Labels
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Domain;
    using Linn.Common.Domain.LinnApps.Services;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;

    public class QcLabelPrinterService : IQcLabelPrinterService
    {
        private readonly ILabelPrinter labelPrinter;

        private readonly IQueryRepository<LabelType> labelTypeRepository;

        private readonly IRepository<Part, string> partsRepository;

        private readonly IDocumentProxy documentProxy;

        private readonly IRepository<Employee, int> employeeRepository;

        public QcLabelPrinterService(
            ILabelPrinter labelPrinter,
            IQueryRepository<LabelType> labelTypeRepository,
            IRepository<Part, string> partsRepository,
            IDocumentProxy documentProxy,
            IRepository<Employee, int> employeeRepository)
        {
            this.labelPrinter = labelPrinter;
            this.labelTypeRepository = labelTypeRepository;
            this.partsRepository = partsRepository;
            this.documentProxy = documentProxy;
            this.employeeRepository = employeeRepository;
        }

        public async Task<ProcessResult> PrintLabels(QcLabelPrintRequest request)
        {
            var result = new ProcessResult(true, string.Empty);

            if (!string.IsNullOrEmpty(request.KardexLocation))
            {
                var labelName = $"KGI{request.OrderNumber}";
                var kardexData = $"\"{request.KardexLocation.Replace("\"", "''")}\",\"{request.ReqNumber}\"";
                var kardexLabelType = await this.labelTypeRepository.FindByAsync(x => x.Code == "KARDEX");
                string printer;

                if (string.IsNullOrEmpty(request.PrinterName))
                {
                    printer = kardexLabelType.DefaultPrinter;
                }
                else
                {
                    var printerLabelType = await this.labelTypeRepository.FindByAsync(
                        x => x.DefaultPrinter.ToLower() == request.PrinterName.ToLower());
                    printer = printerLabelType.DefaultPrinter;
                }

                var kardexResult = await this.labelPrinter.PrintLabelsAsync(
                    labelName,
                    printer,
                    request.Lines == null ? 1 : request.Lines.Count() + 1,
                    kardexLabelType.FileName,
                    kardexData);

                if (!kardexResult.Success)
                {
                    result.Message += kardexResult.Message;
                    result.Success = false;
                }
            }
             
            var labelType = await this.labelTypeRepository.FindByAsync(x => x.Code == request.QcState);
            var employee  = await this.employeeRepository.FindByIdAsync(request.UserNumber);
            var purchaseOrder = await this.documentProxy.GetPurchaseOrder(request.OrderNumber);
            var part = await this.partsRepository.FindByAsync(x => x.PartNumber == request.PartNumber.ToUpper());
            var initials = string.Join(
                string.Empty, 
                employee.Name.Split(' ')
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name[0].ToString().ToUpper()));

            if (request.Lines != null)
            {
                foreach (var line in request.Lines)
                {
                    var printString = string.Empty;
                    switch (request.QcState)
                    {
                        case "QUARANTINE":
                            printString += $"\"{request.DocType}{request.OrderNumber}";
                            printString += "\",\"";
                            printString += part.PartNumber;
                            printString += "\",\"";
                            printString += part.Description;
                            printString += "\",\"";
                            printString += request.DeliveryRef;
                            printString += "\",\"";
                            printString += DateTime.Now.ToString("MMMddyyyy").ToUpper();
                            printString += "\",\"";
                            printString += part.OurUnitOfMeasure;
                            printString += "\",\"";
                            printString += initials;
                            printString += "\",\"";
                            printString += DateTime.Now.ToString("MMMddyyyy").ToUpper();
                            printString += "\",\"";
                            printString += string.IsNullOrEmpty(part.QcInformation) ? "NO QC INFO" : part.QcInformation;
                            printString += "\",\"";
                            printString += purchaseOrder.SupplierId;
                            printString += "\",\"";
                            printString += purchaseOrder.SupplierName;
                            printString += "\",\"";
                            printString += request.Qty;
                            printString += "\",\"";
                            printString += request.NumberOfLines;
                            printString += "\",\"";
                            printString += line.Qty;
                            printString += "\",\"";
                            printString += line.LineNumber;
                            printString += "\",\"";
                            printString += request.QcState;
                            printString += "\",\"";
                            printString += "DATE TESTED";
                            printString += "\",\"";
                            printString += request.ReqNumber;
                            printString += "\"";
                            printString += Environment.NewLine;
                            break;
                        case "PASS":
                            var partMessage = purchaseOrder.Details.First().RohsCompliant == "Y"
                                                   ? "**ROHS Compliant**"
                                                   : null;
                            printString += "\"";
                            printString += request.OrderNumber;
                            printString += "\",\"";
                            printString += request.PartNumber;
                            printString += "\",\"";
                            printString += line.Qty;
                            printString += "\",\"";
                            printString += initials;
                            printString += "\",\"";
                            printString += part.Description;
                            printString += "\",\"";
                            printString += request.ReqNumber;
                            printString += "\",\"";
                            printString += DateTime.Now.ToString("MMMddyyyy").ToUpper();
                            printString += "\",\"";
                            printString += partMessage;
                            printString += "\"";
                            printString += Environment.NewLine;
                            break;
                    }

                    var printer = labelType.DefaultPrinter;
                    if (!string.IsNullOrEmpty(request.PrinterName))
                    {
                        var type = await this.labelTypeRepository.FindByAsync(
                                       x => x.DefaultPrinter.ToLower() == request.PrinterName.ToLower());
                        printer = type.DefaultPrinter;
                    }

                    var linesResult = await this.labelPrinter.PrintLabelsAsync(
                        $"QC {request.OrderNumber}-{line.LineNumber}",
                        printer,
                        1,
                        labelType.FileName,
                        printString);

                    if (!linesResult.Success)
                    {
                        result.Success = false;
                        result.Message += linesResult.Message;
                    }
                }
            }

            return result;
        }
    }
}
