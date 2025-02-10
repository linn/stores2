namespace Linn.Stores2.Resources
{
    using Linn.Common.Resources;

    public class StockStateResource : HypermediaResource
    {
        public string State { get; set; }

        public string Description { get; set; }

        public string QCRequired { get; set; }
    }
}
