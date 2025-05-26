namespace Linn.Stores2.Resources.External
{
    public class SupplierResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public AddressResource OrderAddress { get; set; }
    }
}
