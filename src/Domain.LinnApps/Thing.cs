namespace Linn.Stores2.Domain.LinnApps
{
    public class Thing
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public int? CodeId { get; set; }
        
        public string RecipientName { get; set; }

        public string RecipientAddress { get; set; }
    }
}
