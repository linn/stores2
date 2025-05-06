namespace Linn.Stores2.Resources.Pcas
{
    using Linn.Common.Resources;
    using Linn.Stores2.Resources;

    public class PcasStorageTypeResource : HypermediaResource
    {
        public string BoardCode { get; set; }

        public string StorageTypeCode { get; set; }

        public int? Maximum { get; set; }

        public int? Incr { get; set; }

        public string Remarks { get; set; }

        public string Preference { get; set; }

        public StorageTypeResource StorageType { get; set; }

        public PcasBoardResource PcasBoard { get; set; }
    }
}
