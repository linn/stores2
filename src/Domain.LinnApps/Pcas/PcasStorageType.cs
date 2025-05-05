namespace Linn.Stores2.Domain.LinnApps.Pcas
{
    using Linn.Stores2.Domain.LinnApps.Stock;
    using System;

    public class PcasStorageType
    {
        public PcasStorageType(
            string boardCode,
            string storageTypeCode,
            string maximum,
            int incr,
            string remarks,
            int preference)
        {
            this.BoardCode = boardCode;
            this.StorageTypeCode = storageTypeCode;
            this.Maximum = maximum;
            this.Incr = incr;
            this.Remarks = remarks;
            this.Preference = preference;
        }

        public PcasStorageTypeKey Key { get; set; }

        public string BoardCode { get; set; }

        public string StorageTypeCode { get; set; }

        public string Maximum { get; set; }

        public int Incr { get; set; }

        public string Remarks { get; set; }

        public int Preference { get; set; }

        public StorageType StorageType { get; set; }

        public PcasBoard PcasBoard { get; set; }

        public void Update(
            string maximum,
            int incr,
            string remarks,
            int preference)
        {
            this.Maximum = maximum;
            this.Incr = incr;
            this.Remarks = remarks;
            this.Preference = preference;
        }
    }
}
