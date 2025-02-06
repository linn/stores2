namespace Linn.Stores2.TestData.FunctionCodes
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public static class TestFunctionCodes
    {
        public static readonly StoresFunction Audit =
            new StoresFunction("AUDIT") { Description = "STOCK CHECK ADJUSTMENTS" };

        public static readonly StoresFunction BookWorksOrder =
            new StoresFunction("BOOKWO") { Description = "BOOK IN WORKS ORDER" };

        public static readonly StoresFunction BookFromSupplier =
            new StoresFunction("BOOKSU") { Description = "BOOK IN GOODS FROM SUPPLIER FOR PO" };

        public static readonly StoresFunction LinnDeptReq =
            new StoresFunction("LDREQ") { Description = "BOOK PARTS IN/OUT OF STORES ON REQUISITION" };

        public static readonly StoresFunction SupplierKit =
            new StoresFunction("SUKIT") { Description = "KIT PARTS TO SUPPLIER STORE" };
    }
}
