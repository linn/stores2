namespace Linn.Stores2.Domain.LinnApps.Returns
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.Imports;

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

        public RsnReturnReason AllegedReason { get; set; }

        public ICollection<ImportBookOrderDetail> ImportBookOrderDetails { get; set; }

        public ICollection<ExportReturnDetail> ExportReturnDetails { get; set; }

        public ICollection<RsnReturnInformation> RsnReturns { get; set; }

        public bool ExportRsn => this.SalesOutlet != null && this.SalesOutlet.ExportOutlet;

        public bool IsIpr => this.Ipr == "Y";

        public bool IsReturnForCredit => this.AllegedReason?.IsCredit() ?? false;

        public ExportReturnDetail LastExportReturn()
        {
            if (this.ExportReturnDetails != null && this.ExportReturnDetails.Any())
            {
                return this.ExportReturnDetails.First();
            }

            return null;
        }

        public RsnReturnInformation LastRsnReturn()
        {
            if (this.RsnReturns != null && this.RsnReturns.Any())
            {
                return this.RsnReturns.First();
            }

            return null;
        }

        public bool HasCustomsInformation()
        {
            return this.LastExportReturn() != null || this.LastRsnReturn() != null;
        }

        public Currency CustomsCurrency()
        {
            var exportReturn = this.LastExportReturn();
            if (exportReturn != null)
            {
                return exportReturn.ExportReturn.Currency;
            }

            var rsnReturn = this.LastRsnReturn();
            if (rsnReturn != null)
            {
                return rsnReturn.Currency;
            }

            return null;
        }

        public decimal? CustomsValue()
        {
            var exportReturn = this.LastExportReturn();
            if (exportReturn != null)
            {
                return exportReturn.CustomsValue;
            }

            var rsnReturn = this.LastRsnReturn();
            return rsnReturn?.CustomsValue;
        }

        // help find which CPC number is should be using
        public string ImportScheme()
        {
            if (this.IsIpr && !this.IsReturnForCredit)
            {
                return "IPR";
            }

            return "BRG";
        }
    }
}
