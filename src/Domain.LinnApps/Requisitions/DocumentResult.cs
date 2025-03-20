namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using Linn.Stores2.Domain.LinnApps.Parts;

    public class DocumentResult
    {
        public DocumentResult(string name, int number, int? line, decimal? qty, string partNumber)
        {
            this.DocumentName = name;
            this.DocumentNumber = number;
            this.LineNumber = line;
            this.Quantity = qty;
            this.PartNumber = partNumber;
        }

        public string DocumentName { get; protected set; }

        public int DocumentNumber { get; protected set; }

        public int? LineNumber { get; protected set; }

        public decimal? Quantity { get; protected set; }

        public string PartNumber { get; protected set; }

        public Part Part { get; set; }
    }
}
