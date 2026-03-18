namespace Linn.Stores2.Domain.LinnApps.Returns
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ExportReturnDetail
    {
        public int ReturnId { get; set; }

        public ExportReturn ExportReturn { get; set; }

        public int RsnNumber { get; set; }

        public string ArticleNumber { get; set; }

        public int LineNo { get; set; }

        public int Qty { get; set; }

        public string Description { get; set; }

        public decimal? CustomsValue { get; set; }

        public decimal? BaseCustomsValue { get; set; }

        public int? NumCartons { get; set; }

        public double? Weight { get; set; }

        public double? Width { get; set; }

        public double? Height { get; set; }

        public double? Depth { get; set; }
    }
}
