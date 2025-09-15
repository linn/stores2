namespace Linn.Service.Domain.LinnApps
{
    using System;

    public class SalesArticle
    {
        public string ArticleNumber { get; set; }

        public string Description { get; set; }

        public DateTime? PhaseOutDate { get; set; }

        public int TariffId { get; set; }

        public Tariff Tariff { get; set; }

        public decimal? Weight { get; set; }

        public string ArticleType { get; set; }

        public string TypeOfSerialNumber { get; set; }
    }
}
