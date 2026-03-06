namespace Linn.Stores2.Resources
{
    public class TotalResource
    {
        public TotalResource(decimal total)
        {
            this.Total = total;
        }

        public decimal Total { get; protected init; }
    }
}
