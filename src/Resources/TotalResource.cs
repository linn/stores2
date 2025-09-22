namespace Linn.Stores2.Resources
{
    public class TotalResource
    {
        public decimal Total { get; protected init; }

        public TotalResource(decimal total)
        {
            this.Total = total;
        }
    }
}
