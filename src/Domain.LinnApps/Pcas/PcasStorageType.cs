namespace Linn.Stores2.Domain.LinnApps.Pcas
{
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class PcasStorageType
    {
        public PcasStorageTypeKey Key { get; set; }

        public string BoardCode { get; set; }

        public string StorageTypeCode { get; set; }

        public string Maximum { get; set; }

        public int Incr { get; set; }

        public string Remarks { get; set; }

        public int Preference { get; set; }

        public StorageType StorageType { get; set; }

        public PcasBoard PcasBoard { get; set; }
    }
}
