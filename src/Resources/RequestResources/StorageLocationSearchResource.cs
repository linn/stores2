namespace Linn.Stores2.Resources.RequestResources
{
    public class StorageLocationSearchResource
    {
        public bool? IncludeInvalid { get; set; }

        public string SiteCode { get; set; }
        
        public string StorageAreaCode { get; set; }

        public string LocationCode { get; set; }
    }
}
