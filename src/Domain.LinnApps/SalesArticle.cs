namespace Linn.Stores2.Domain.LinnApps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.Parts;

    public class SalesArticle
    {
        public string ArticleNumber { get; set; }

        public string Description { get; set; }

        public DateTime? PhaseOutDate { get; set; }

        public Tariff Tariff { get; set; }

        public int TariffId { get; set; }

        public decimal? Weight { get; set; }

        public Country CountryOfOrigin { get; set; }
    }
}
