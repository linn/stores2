namespace Linn.Stores2.Resources.Parts
{ 
    using Linn.Common.Resources;

    public class PartsStorageTypeResource : HypermediaResource
    {
            public string PartNumber { get; set; }

            public PartResource Part { get; set; }

            public string StorageTypeCode { get; set; }

            public StorageTypeResource StorageType { get; set; }

            public string Remarks { get; set; }

            public int Maximum { get; set; }

            public int Incr { get; set; }

            public string Preference { get; set; }

            public int BridgeId { get; set; }
        }
    }