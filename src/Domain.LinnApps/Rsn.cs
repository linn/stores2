namespace Linn.Stores2.Domain.LinnApps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Imports;

    public class Rsn
    {
        public int RsnNumber { get; set; }

        public int AccountId { get; set; }

        public int? OutletNumber { get; set; }

        public SalesOutlet SalesOutlet { get; set; }

        public int Quantity { get; set; }

        public string Ipr { get; set; }

        public string ArticleNumber { get; set; }

        public SalesArticle SalesArticle { get; set; }

        public ICollection<ImportBookOrderDetail> ImportBookOrderDetails { get; set; }

        public bool ExportRsn => this.SalesOutlet != null && this.SalesOutlet.ExportOutlet;
    }
}
