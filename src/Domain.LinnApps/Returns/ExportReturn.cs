namespace Linn.Stores2.Domain.LinnApps.Returns
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ExportReturn
    {
        public int ReturnId { get; set; }

        public Currency Currency { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
