namespace Linn.Stores2.Resources.GoodsIn
{
    public class GoodsInLogEntrySearchResource
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int? CreatedBy { get; set; }

        public string ArticleNumber { get; set; }

        public decimal? Quantity { get; set; }

        public int? OrderNumber { get; set; }

        public int? ReqNumber { get; set; }

        public string StoragePlace { get; set; }
    }
}
