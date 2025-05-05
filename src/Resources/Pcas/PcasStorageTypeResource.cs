namespace Linn.Stores2.Resources.Pcas
{
    using Linn.Stores2.Resources;

    public class PcasStorageTypeResource
    {
        public string BoardCode { get; set; }

        public string StorageTypeCode { get; set; }

        public string Maximum { get; set; }

        public int Incr { get; set; }

        public string Remarks { get; set; }

        public int Preference { get; set; }

        public StorageTypeResource StorageType { get; set; }

        public PcasBoardResource PcasBoard { get; set; }
    }
}
