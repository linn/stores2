namespace Linn.Stores2.Resources.External
{
    public class CreditNoteDetailResource
    {
        public int DocumentNumber { get; set; }

        public string DocumentType { get; set; }

        public int LineNumber { get; set; }

        public string LineType { get; set; }

        public string ArticleNumber { get; set; }

        public decimal Quantity { get; set; }
    }
}
