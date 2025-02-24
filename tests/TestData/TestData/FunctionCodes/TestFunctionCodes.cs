namespace Linn.Stores2.TestData.FunctionCodes
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public static class TestFunctionCodes
    {
        public static readonly StoresFunction Audit =
            new StoresFunction("AUDIT")
            {
                Description = "STOCK CHECK ADJUSTMENTS",
                Document1RequiredFlag = "N"
            };

        public static readonly StoresFunction BookWorksOrder =
            new StoresFunction("BOOKWO")
            {
                Description = "BOOK IN WORKS ORDER",
                Document1RequiredFlag = "Y",
                Document1Text = "Works Order"
            };

        public static readonly StoresFunction BookFromSupplier =
            new StoresFunction("BOOKSU")
            {
                Description = "BOOK IN GOODS FROM SUPPLIER FOR PO",
                Document1RequiredFlag = "Y",
                Document1Text = "Order Number"
            };

        public static readonly StoresFunction LinnDeptReq =
            new StoresFunction("LDREQ")
            {
                Description = "BOOK PARTS IN/OUT OF STORES ON REQUISITION",
                Document1RequiredFlag = "N"
            };

        public static readonly StoresFunction SupplierKit =
            new StoresFunction("SUKIT")
            {
                Description = "KIT PARTS TO SUPPLIER STORE",
                Document1RequiredFlag = "Y",
                Document1Text = "Order Number"
            };

        public static readonly StoresFunction LoanOut =
            new StoresFunction("LOAN OUT")
            {
                Description = "BOOK OUT PRODUCTS TO LOAN ACCOUNT",
                Document1RequiredFlag = "Y",
                Document1Text = "Loan Number"
            };
    }
}
