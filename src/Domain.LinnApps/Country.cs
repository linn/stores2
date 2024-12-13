namespace Linn.Stores2.Domain.LinnApps
{
    public class Country
    {
        public Country()
        {
        }

        public Country(string countryCode, string name)
        {
            this.CountryCode = countryCode;
            this.Name = name;
        }

        public string CountryCode { get; protected set; }

        public string Name { get; protected set; }
    }
}
