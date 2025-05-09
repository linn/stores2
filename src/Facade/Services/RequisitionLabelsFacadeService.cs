namespace Linn.Stores2.Facade.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Labels;
    using Linn.Stores2.Resources.Requisitions;
    
    public class RequisitionLabelsFacadeService : IRequisitionLabelsFacadeService
    {
        private readonly IQcLabelPrinterService domainService;

        public RequisitionLabelsFacadeService(IQcLabelPrinterService domainService)
        {
            this.domainService = domainService;
        }
        
        public async Task<IResult<ProcessResultResource>> PrintQcLables(QcLabelPrintRequestResource request)
        {
            var result = await this.domainService.PrintLabels(new QcLabelPrintRequest
            {
                DocType = request.DocType,
                PartNumber = request.PartNumber,
                DeliveryRef = request.DeliveryRef,
                Qty = request.Qty,
                UserNumber = request.UserNumber,
                OrderNumber = request.OrderNumber,
                NumberOfLines = request.NumberOfLines,
                QcState = request.QcState,
                ReqNumber = request.ReqNumber,
                KardexLocation = request.KardexLocation,
                Lines = request.Lines?.Select((l, i) => new LabelLine(l, i)),
                PrinterName = request.PrinterName
            });
            
            return new SuccessResult<ProcessResultResource>(
                new ProcessResultResource(result.Success, result.Message));
        }
    }
}
