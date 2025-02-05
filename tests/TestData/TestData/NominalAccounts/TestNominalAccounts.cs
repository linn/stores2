namespace Linn.Stores2.TestData.NominalAccounts
{
    using Linn.Stores2.Domain.LinnApps.Accounts;

    public static class TestNominalAccounts
    {
        public static readonly NominalAccount TestNomAcc =
            new NominalAccount(new Department("0000001234", "TEST DEPT", "Y"),
                new Nominal("0000005678", "TEST NOM"), "Y");

        // any stock transaction for Parts that are Raw Material will have this for one side
        public static readonly NominalAccount AssetsRawMat =
            new NominalAccount(new Department("0000002508", "ASSETS", "N"),
                new Nominal("0000007617", "RAW MATERIALS"), "Y");

        // any stock transaction for Parts that are Finished Goods will have this for one side
        public static readonly NominalAccount FinAssWipUsed =
            new NominalAccount(new Department("0000042808", "FINAL ASSEMBLY", "N"),
                new Nominal("0000000435", "WIP USED"), "Y");

        // transactions against purchase orders will probably use this
        public static readonly NominalAccount UninvoicedCreditors =
            new NominalAccount(new Department("0000002508", "ASSETS", "N"),
                new Nominal("0000009279", "UNINVOICED CREDITORS"), "Y");

        // used in material price variance transactions
        public static readonly NominalAccount MaterialPriceVariances =
            new NominalAccount(new Department("0000002302", "OTHER OVERHEADS", "N"),
                new Nominal("0000012926", "MATERIAL PRICE VARIANCES"), "Y");
    }
}
