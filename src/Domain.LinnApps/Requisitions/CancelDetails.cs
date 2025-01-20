namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;

    public class CancelDetails
    {
        public int Id { get; set; }

        public int ReqNumber { get; set; }

        public int? LineNumber { get; set; }

        public int CancelledBy { get; set; }

        public DateTime DateCancelled { get; set; }

        public string Reason { get; set; }
    }
}
