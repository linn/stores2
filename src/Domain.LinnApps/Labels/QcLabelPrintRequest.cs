namespace Linn.Stores2.Domain.LinnApps.Labels
{
    using System.Collections.Generic;

    public class QcLabelPrintRequest
    {
        public string DocType { get; set; }

        public string PartNumber { get; set; }

        public string DeliveryRef { get; set; }

        public int Qty { get; set; }

        public int UserNumber { get; set; }

        public int OrderNumber { get; set; }

        public int NumberOfLines { get; set; }

        public string QcState { get; set; }

        public int ReqNumber { get; set; }

        public string KardexLocation { get; set; }

        public IEnumerable<LabelLine> Lines { get; set; }

        public string PrinterName { get; set; }
    }
}
