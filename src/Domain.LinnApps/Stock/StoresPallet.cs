namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System;

    public class StoresPallet
    {
        public int PalletNumber { get; set; }

        public int LocationId { get; set; }

        public string Description { get; set; }

        public DateTime? DateInvalid { get; set; }

        public string TypeOfStock { get; set; }

        public string StockState { get; set; }

        public string MixStates { get; set; }
    }
}
