namespace Linn.Stores2.Domain.LinnApps.Imports.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.Returns;

    public class ImportOrderDetailCandidate
    {
        public ImportOrderDetailCandidate()
        {
            this.Qty = 1;
        }

        public ImportOrderDetailCandidate(Rsn rsn)
        {
            this.LineType = "RSN";
            this.Rsn = rsn;
            this.Qty = rsn.Quantity;
            this.OrderDescription = rsn.SalesArticle?.Description;
            this.TariffCode = rsn.SalesArticle?.Tariff?.TariffCode;
            this.CountryOfOrigin = rsn.SalesArticle?.CountryOfOrigin;
        }

        public int ImportBookId { get; set; }

        public string LineType { get; set; }

        public int Qty { get; set; }

        public string OrderDescription { get; set; }

        public string TariffCode { get; set; }

        public Country CountryOfOrigin { get; set; }

        public Rsn Rsn { get; set; }
    }
}
