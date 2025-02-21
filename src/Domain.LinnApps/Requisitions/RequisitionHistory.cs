namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;

    public class RequisitionHistory
    {
        public int Id { get; set; }

        public int ReqNumber { get; set; }

        public string Action { get; set; }

        public DateTime DateChanged { get; set; }
        
        public int By { get; set; }

        public string FunctionCode { get; set; }
    }
}
