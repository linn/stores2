namespace Linn.Stores2.Domain.LinnApps
{
    public class Country
    {
        public Country()
        {
        }

        public Country(
            string countryCode, 
            string name, 
            string displayName = null, 
            string euMember = "N")
        {
            this.CountryCode = countryCode;
            this.Name = name;
            this.EuMember = euMember;
            this.DisplayName = displayName ?? name;
        }

        public string CountryCode { get; protected set; }

        public string Name { get; protected set; }

        // nicer version of name which isn't ALL CAPS LIKE AN ANGRY TWEET i.e. United Kingdom not UNITED KINGDOM
        public string DisplayName { get; protected set; }

        // still called EecMember in database which the EU hasn't been since 1993
        public string EuMember { get; protected set; }

        public bool IsEuMember => this.EuMember == "Y";
    }
}
