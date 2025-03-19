namespace Linn.Stores2.TestData.FunctionCodes
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Transactions;

    public static class TestFunctionCodes
    {
        public static readonly StoresFunction Adjust =
            new StoresFunction("ADJUST")
            {
                Description = "ADJUST PARTS UP/DOWN IN STOCK",
                DepartmentNominalRequired = "Y",
                Document1RequiredFlag = "N",
                Document1LineRequiredFlag = "N",
                FromStateRequired = "N",
                PartSource = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>()
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "ADJUST",
                        Seq = 1,
                        TransactionDefinition = TestTransDefs.StockToAdjust,
                        TransactionCode = TestTransDefs.StockToAdjust.TransactionCode
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "ADJUST",
                        Seq = 2,
                        TransactionDefinition = TestTransDefs.AdjustToStock,
                        TransactionCode = TestTransDefs.AdjustToStock.TransactionCode
                    }
                }
            };

        public static readonly StoresFunction AdjustQC =
            new StoresFunction("ADJUST QC")
            {
                Description = "ADJUST PARTS UP/DOWN IN INSPECTION",
                DepartmentNominalRequired = "Y",
                Document1RequiredFlag = "N",
                Document1LineRequiredFlag = "N",
                FromStateRequired = "Y",
                PartSource = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>()
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "ADJUST QC",
                        Seq = 1,
                        TransactionDefinition = TestTransDefs.QCToAdjust,
                        TransactionCode = TestTransDefs.StockToAdjust.TransactionCode
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "ADJUST QC",
                        Seq = 2,
                        TransactionDefinition = TestTransDefs.AdjustToQC,
                        TransactionCode = TestTransDefs.AdjustToStock.TransactionCode
                    }
                }
            };

        public static readonly StoresFunction Audit =
            new StoresFunction("AUDIT")
            {
                Description = "STOCK CHECK ADJUSTMENTS",
                Document1RequiredFlag = "N",
                Document1LineRequiredFlag = "N",
                FromStateRequired = "N",
                PartSource = "N"
            };

        public static readonly StoresFunction BookWorksOrder =
            new StoresFunction("BOOKWO")
            {
                Description = "BOOK IN WORKS ORDER",
                Document1RequiredFlag = "Y",
                Document1LineRequiredFlag = "N",
                Document1Text = "Works Order",
                FromStateRequired = "N",
                PartSource = "WO"
            };

        public static readonly StoresFunction BookFromSupplier =
            new StoresFunction("BOOKSU")
            {
                Description = "BOOK IN GOODS FROM SUPPLIER FOR PO",
                Document1RequiredFlag = "Y",
                Document1LineRequiredFlag = "Y",
                Document1Text = "Order Number",
                FromStateRequired = "N",
                PartSource = "PO",
                TransactionsTypes = new List<StoresFunctionTransaction>()
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "BOOKSU",
                        Seq = 1,
                        TransactionDefinition = TestTransDefs.SupplierToStores,
                        TransactionCode = TestTransDefs.SupplierToStores.TransactionCode
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "BOOKSU",
                        Seq = 2,
                        TransactionDefinition = TestTransDefs.MaterialVarianceBelowStd,
                        TransactionCode = TestTransDefs.MaterialVarianceBelowStd.TransactionCode
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "BOOKSU",
                        Seq = 3,
                        TransactionDefinition = TestTransDefs.SupplierToQC,
                        TransactionCode = TestTransDefs.SupplierToQC.TransactionCode
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "BOOKSU",
                        Seq = 4,
                        TransactionDefinition = TestTransDefs.SupplierToNowhere,
                        TransactionCode = TestTransDefs.SupplierToNowhere.TransactionCode
                    }
                }
            };

        public static readonly StoresFunction CustomerReturn =
            new StoresFunction("CUSTRET")
            {
                Description = "RETURN GOODS FROM CUSTOMER TO STOCK/INSPECTION",
                DepartmentNominalRequired = "N",
                Document1RequiredFlag = "Y",
                Document1LineRequiredFlag = "O",
                Document2RequiredFlag = "Y",
                FromStateRequired = "N",
                ToStateRequired = "Y",
                ToStockPoolRequired = "Y",
                PartSource = "C",
                TransactionsTypes = new List<StoresFunctionTransaction>()
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "CUSTRET",
                        Seq = 1,
                        TransactionDefinition = TestTransDefs.CustomerToInspection,
                        TransactionCode = TestTransDefs.CustomerToInspection.TransactionCode
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "CUSTRET",
                        Seq = 2,
                        TransactionDefinition = TestTransDefs.CustomerToGoodStock,
                        TransactionCode = TestTransDefs.CustomerToGoodStock.TransactionCode
                    }
                }
            };

        public static readonly StoresFunction LinnDeptReq =
            new StoresFunction("LDREQ")
            {
                Description = "BOOK PARTS IN/OUT OF STORES ON REQUISITION",
                DepartmentNominalRequired = "Y",
                Document1RequiredFlag = "N",
                Document1LineRequiredFlag = "N",
                FromStateRequired = "N",
                PartSource = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>()
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "LDREQ",
                        Seq = 1,
                        TransactionDefinition = TestTransDefs.StockToLinnDept,
                        TransactionCode = TestTransDefs.StockToLinnDept.TransactionCode
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "LDREQ",
                        Seq = 2,
                        TransactionDefinition = TestTransDefs.LinnDeptToStock,
                        TransactionCode = TestTransDefs.LinnDeptToStock.TransactionCode
                    }
                }
            };

        public static readonly StoresFunction LoanOut =
            new StoresFunction("LOAN OUT")
            {
                Description = "BOOK OUT PRODUCTS TO LOAN ACCOUNT",
                Document1RequiredFlag = "Y",
                Document1Text = "Loan Number",
                Document1LineRequiredFlag = "N",
                DepartmentNominalRequired = "N",
                FromStateRequired = "N",
                PartSource = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>()
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "LOAN OUT",
                        Seq = 1,
                        TransactionDefinition = TestTransDefs.StockToLoan,
                        TransactionCode = TestTransDefs.StockToLoan.TransactionCode
                    }
                }
            };

        public static readonly StoresFunction Rsn =
            new StoresFunction("RSN")
            {
                Description = "BOOKS PARTS TO A RSN",
                Document1RequiredFlag = "Y",
                Document1Text = "RSN Number",
                Document1LineRequiredFlag = "N",
                DepartmentNominalRequired = "N",
                FromStateRequired = "N",
                PartSource = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>()
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "RSN",
                        Seq = 1,
                        ReqType = "F",
                        TransactionDefinition = TestTransDefs.StockToRSN,
                        TransactionCode = TestTransDefs.StockToRSN.TransactionCode
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "RSN",
                        Seq = 2,
                        ReqType = "O",
                        TransactionDefinition = TestTransDefs.RSNToStock,
                        TransactionCode = TestTransDefs.RSNToStock.TransactionCode
                    }
                }
            };

        public static readonly StoresFunction SupplierKit =
            new StoresFunction("SUKIT")
            {
                Description = "KIT PARTS TO SUPPLIER STORE",
                Document1RequiredFlag = "Y",
                Document1LineRequiredFlag = "N",
                Document1Text = "Order Number",
                FromStateRequired = "N",
                PartSource = "PO"
            };

        public static readonly StoresFunction WriteOff =
            new StoresFunction("WOFF")
            {
                Description = "WRITE OFF/ON PARTS IN STOCK",
                DepartmentNominalRequired = "Y",
                Document1RequiredFlag = "N",
                Document1LineRequiredFlag = "N",
                FromStateRequired = "N",
                PartSource = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>()
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "WOFF",
                        Seq = 1,
                        TransactionDefinition = TestTransDefs.WriteOff,
                        TransactionCode = TestTransDefs.WriteOff.TransactionCode
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "WOFF",
                        Seq = 2,
                        TransactionDefinition = TestTransDefs.WriteOn,
                        TransactionCode = TestTransDefs.WriteOn.TransactionCode
                    }
                }
            };

        public static readonly StoresFunction GistPo =
            new StoresFunction("GIST PO")
                {
                    QuantityRequired = "Y",
                    Description = "BOOK STOCK INTO STORES FROM QC ON A PO",
                    FromStockPoolRequired = "Y",
                    ToStockPoolRequired = "Y",
                    DepartmentNominalRequired = "N",
                    Document1RequiredFlag = "Y",
                    Document1LineRequiredFlag = "O",
            };
    }
}
