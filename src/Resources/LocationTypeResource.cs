namespace Linn.Stores2.Resources
{
    using Linn.Common.Resources;

    public class LocationTypeResource : HypermediaResource
    {
        public LocationTypeResource()
        {
        }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}
