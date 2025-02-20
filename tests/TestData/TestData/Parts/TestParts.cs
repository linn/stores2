namespace Linn.Stores2.TestData.Parts
{
    using Linn.Stores2.Domain.LinnApps.Parts;

    public static class TestParts
    {
        public static readonly Part Cap003 =
            new Part
                {
                PartNumber = "CAP 003",
                Description = "Test Capacitor",
                RawOrFinished = "R",
                AccountingCompanyCode = "LINN"
            };

        public static readonly Part SelektHub =
            new Part
                {
                PartNumber = "SK HUB",
                Description = "SELEKT DSM HUB",
                RawOrFinished = "F",
                AccountingCompanyCode = "LINN"
            };
    }
}
