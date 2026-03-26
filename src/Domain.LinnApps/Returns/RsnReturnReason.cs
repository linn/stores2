namespace Linn.Stores2.Domain.LinnApps.Returns
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RsnReturnReason
    {
        public string ReasonCode { get; set; }

        public string Description { get; set; }

        public DateTime? DateInvalid { get; set; }

        public string RealReasonFlag { get; set; }

        public string ReasonCategory { get; set; }

        public bool IsUpgrade() => this.ReasonCategory == "Upgrade";

        public bool IsRepair() => this.ReasonCategory == "Repair";

        public bool IsCredit() => this.ReasonCategory == "Credit";
    }
}
