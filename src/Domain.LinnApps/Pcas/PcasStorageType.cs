namespace Linn.Stores2.Domain.LinnApps.Pcas
{
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class PcasStorageType
    {
        public PcasStorageType(
            string boardCode,
            string storageTypeCode,
            int maximum,
            int incr,
            string remarks,
            string preference)
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

        public int Maximum { get; set; }

        public int Incr { get; set; }

        public string Remarks { get; set; }

        public string Preference { get; set; }

        public StorageType StorageType { get; set; }

        public PcasBoard PcasBoard { get; set; }

        public void Update(
            int maximum,
            int incr,
            string remarks,
            string preference)
        {
            this.Maximum = maximum;
            this.Incr = incr;
            this.Remarks = remarks;
            this.Preference = preference;
        }
    }
}
