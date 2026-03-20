namespace Linn.Stores2.Domain.LinnApps.Returns
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RsnReturnInformation
    {
        public int RsnNumber { get; set; }

        public int AccountId { get; set; }

        public DateTime DateGenerated { get; set; }

        public decimal CustomsValue { get; set; }

        public Currency Currency { get; set; }

        public decimal? ExchangeRate { get; set; }

        public string ArticleNumber { get; set; }
    }
}
