namespace Linn.Stores2.Domain.LinnApps.Labels
{
    using System.Threading.Tasks;

    using Linn.Common.Domain;
    using Linn.Common.Domain.LinnApps.Services;

    public class QcLabelPrinterService : IQcLabelPrinterService
    {
        private readonly ILabelPrinter labelPrinter;

        public QcLabelPrinterService(ILabelPrinter labelPrinter)
        {
            this.labelPrinter = labelPrinter;
        }

        public Task<ProcessResult> PrintLabels(QcLabelPrintRequest data)
        {
            throw new System.NotImplementedException();
        }
    }
}
