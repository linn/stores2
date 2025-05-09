namespace Linn.Stores2.Domain.LinnApps.Labels
{
    using System.Threading.Tasks;

    using Linn.Common.Domain;

    public interface IQcLabelPrinterService
    {
        Task<ProcessResult> PrintLabels(QcLabelPrintRequest data);
    }
}
