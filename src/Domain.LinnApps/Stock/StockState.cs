namespace Linn.Stores2.Domain.LinnApps.Stock
{
    public class StockState
    {
        public StockState()
        {
        }

        public StockState(string state, string description)
        {
            this.State = state;
            this.Description = description;
            this.QCRequired = "N";
        }

        public string State { get; set; }

        public string Description { get; set; }

        public string QCRequired { get; set; }
    }
}
