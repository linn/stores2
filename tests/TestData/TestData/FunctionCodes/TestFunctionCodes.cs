namespace Linn.Stores2.TestData.FunctionCodes
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public static class TestFunctionCodes
    {
        public static readonly StoresFunctionCode Audit =
            new StoresFunctionCode("AUDIT") { Description = "STOCK CHECK ADJUSTMENTS" };

        public static readonly StoresFunctionCode BookWorksOrder =
            new StoresFunctionCode("BOOKWO") { Description = "BOOK IN WORKS ORDER" };

        public static readonly StoresFunctionCode BookFromSupplier =
            new StoresFunctionCode("BOOKSU") { Description = "BOOK IN GOODS FROM SUPPLIER FOR PO" };

        public static readonly StoresFunctionCode LinnDeptReq =
            new StoresFunctionCode("LDREQ") { Description = "BOOK PARTS IN/OUT OF STORES ON REQUISITION" };

        public static readonly StoresFunctionCode SupplierKit =
            new StoresFunctionCode("SUKIT") { Description = "KIT PARTS TO SUPPLIER STORE" };
    }
}
