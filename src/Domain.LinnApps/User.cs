namespace Linn.Stores2.Domain.LinnApps
{
    using System.Collections.Generic;

    public class User
    {
        public int UserNumber { get; set; }
        
        public IEnumerable<string> Privileges { get; set; }
    }
}
