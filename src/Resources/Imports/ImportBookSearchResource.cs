namespace Linn.Stores2.Resources.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ImportBookSearchResource
    {
        public string TransportBillNumber { get; set; }

        public string CustomsEntryCode { get; set; }

        public int? RsnNumber { get; set; }

        public int? PONumber { get; set; }
    }
}
