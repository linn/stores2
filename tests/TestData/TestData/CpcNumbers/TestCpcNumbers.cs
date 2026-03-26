namespace Linn.Stores2.TestData.CpcNumbers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.Imports;

    public static class TestCpcNumbers
    {
        public static ImportBookCpcNumber MaterialCpc => new ImportBookCpcNumber
        {
            CpcNumber = 4,
            Description = "40 00 000",
            ReasonForImport = "Material"
        };

        public static ImportBookCpcNumber IPRCpc => new ImportBookCpcNumber
        {
            CpcNumber = 13,
            Description = "51 00 000",
            ReasonForImport = "IPR"
        };

        public static ImportBookCpcNumber BRGCpc => new ImportBookCpcNumber
        {
            CpcNumber = 13,
            Description = "61 10 F05 - BRG",
            ReasonForImport = "BRG"
        };

        public static IList<ImportBookCpcNumber> CpcNumbers => new List<ImportBookCpcNumber>
        {
            MaterialCpc,
            IPRCpc,
            BRGCpc
        };
    }
}
