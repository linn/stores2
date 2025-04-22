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
                BatchRequired = "N",
                DepartmentNominalRequired = "Y",
                Document1RequiredFlag = "N",
                Document1LineRequiredFlag = "N",
                FromStateRequired = "N",
                FunctionType = "M",
                ManualPickRequired = "M",
                PartSource = "N",
                ToStateRequired = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "ADJUST",
                        Seq = 1,
                        TransactionDefinition = TestTransDefs.StockToAdjust,
                        TransactionCode = TestTransDefs.StockToAdjust.TransactionCode,
                        ReqType = "F"
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "ADJUST",
                        Seq = 2,
                        TransactionDefinition = TestTransDefs.AdjustToStock,
                        TransactionCode = TestTransDefs.AdjustToStock.TransactionCode,
                        ReqType = "O"
                    }
                }
            };

        public static readonly StoresFunction AdjustQC =
            new StoresFunction("ADJUST QC")
            {
                Description = "ADJUST PARTS UP/DOWN IN INSPECTION",
                BatchRequired = "N",
                DepartmentNominalRequired = "Y",
                Document1RequiredFlag = "N",
                Document1LineRequiredFlag = "N",
                FromStateRequired = "Y",
                FunctionType = "M",
                ManualPickRequired = "M",
                PartSource = "N",
                ToStateRequired = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>
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
                BatchRequired = "N",
                Document1RequiredFlag = "N",
                Document1LineRequiredFlag = "N",
                FromStateRequired = "N",
                FunctionType = "M",
                ManualPickRequired = "M",
                PartSource = "N",
                ToStateRequired = "N"
            };

        public static readonly StoresFunction BookWorksOrder =
            new StoresFunction("BOOKWO")
            {
                Description = "BOOK IN WORKS ORDER",
                BatchRequired = "N",
                Document1RequiredFlag = "Y",
                Document1LineRequiredFlag = "N",
                Document1Text = "Works Order",
                FromStateRequired = "N",
                FunctionType = "A",
                ManualPickRequired = "X",
                PartSource = "WO",
                ToStateRequired = "O"
            };

        public static readonly StoresFunction BookFromSupplier =
            new StoresFunction("BOOKSU")
            {
                Description = "BOOK IN GOODS FROM SUPPLIER FOR PO",
                BatchRequired = "N",
                Document1RequiredFlag = "Y",
                Document1LineRequiredFlag = "Y",
                Document1Text = "Order Number",
                FromStateRequired = "N",
                FunctionType = "A",
                ManualPickRequired = "X",
                PartSource = "PO",
                ToStateRequired = "O",
                TransactionsTypes = new List<StoresFunctionTransaction>
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
                BatchRequired = "N",
                DepartmentNominalRequired = "N",
                Document1RequiredFlag = "Y",
                Document1LineRequiredFlag = "O",
                Document2RequiredFlag = "Y",
                FromStateRequired = "N",
                FunctionType = "A",
                ToStateRequired = "Y",
                ToLocationRequired = "O",
                ToStockPoolRequired = "Y",
                ToStockPool = "LINN",
                ManualPickRequired = "M",
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
                BatchRequired = "N",
                DepartmentNominalRequired = "Y",
                Document1RequiredFlag = "N",
                LinesRequired = "Y",
                Document1LineRequiredFlag = "N",
                FromStateRequired = "N",
                FunctionType = "M",
                ManualPickRequired = "A",
                PartSource = "N",
                ToStateRequired = "N",
                ToStockPoolRequired = "O",
                TransactionsTypes = new List<StoresFunctionTransaction>
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "LDREQ",
                        Seq = 1,
                        ReqType = "F",
                        TransactionDefinition = TestTransDefs.StockToLinnDept,
                        TransactionCode = TestTransDefs.StockToLinnDept.TransactionCode
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "LDREQ",
                        Seq = 2,
                        ReqType = "O",
                        TransactionDefinition = TestTransDefs.LinnDeptToStock,
                        TransactionCode = TestTransDefs.LinnDeptToStock.TransactionCode
                    }
                }
            };

        public static readonly StoresFunction LoanOut =
            new StoresFunction("LOAN OUT")
            {
                Description = "BOOK OUT PRODUCTS TO LOAN ACCOUNT",
                BatchRequired = "N",
                Document1RequiredFlag = "Y",
                Document1Text = "Loan Number",
                Document1LineRequiredFlag = "N",
                DepartmentNominalRequired = "N",
                FromStateRequired = "N",
                FunctionType = "M",
                ManualPickRequired = "M",
                PartSource = "N",
                ToStateRequired = "N",
                ToStockPoolRequired = "O",
                TransactionsTypes = new List<StoresFunctionTransaction>
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
                BatchRequired = "N",
                Document1RequiredFlag = "Y",
                Document1Text = "RSN Number",
                Document1LineRequiredFlag = "N",
                DepartmentNominalRequired = "N",
                FromStateRequired = "N",
                FunctionType = "M",
                ManualPickRequired = "A",
                PartSource = "N",
                ToStateRequired = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>
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
                BatchRequired = "N",
                DepartmentNominalRequired = "N",
                Document1RequiredFlag = "Y",
                Document1LineRequiredFlag = "N",
                Document1Text = "Order Number",
                FromStateRequired = "N",
                FunctionType = "A",
                ManualPickRequired = "A",
                PartSource = "PO",
                ToLocationRequired = "Y",
                ToStateRequired = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "SUKIT",
                        TransactionDefinition = TestTransDefs.StockToSupplierKit,
                        TransactionCode = TestTransDefs.StockToSupplierKit.TransactionCode
                    }
                }
            };

        public static readonly StoresFunction WriteOff =
            new StoresFunction("WOFF")
            {
                Description = "WRITE OFF/ON PARTS IN STOCK",
                BatchRequired = "N",
                DepartmentNominalRequired = "Y",
                Document1RequiredFlag = "N",
                Document1LineRequiredFlag = "N",
                FromStateRequired = "N",
                FunctionType = "M",
                ManualPickRequired = "M",
                PartSource = "N",
                ToStockPoolRequired = "O",
                ToStateRequired = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "WOFF",
                        Seq = 1,
                        TransactionDefinition = TestTransDefs.WriteOff,
                        TransactionCode = TestTransDefs.WriteOff.TransactionCode,
                        ReqType = "F"
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "WOFF",
                        Seq = 2,
                        TransactionDefinition = TestTransDefs.WriteOn,
                        TransactionCode = TestTransDefs.WriteOn.TransactionCode,
                        ReqType = "O"
                    }
                }
            };

        public static readonly StoresFunction GistPo =
            new StoresFunction("GIST PO")
                {
                    QuantityRequired = "Y",
                    Description = "BOOK STOCK INTO STORES FROM QC ON A PO",
                    BatchRequired = "Y",
                    FromStockPoolRequired = "Y",
                    ToStockPoolRequired = "Y",
                    DepartmentNominalRequired = "N",
                    ManualPickRequired = "M",
                    PartSource = "PO",
                    Document1RequiredFlag = "Y",
                    Document1LineRequiredFlag = "O",
                    FunctionType = "A",
                    ToStateRequired = "X",
                    TransactionsTypes = new List<StoresFunctionTransaction>
                                            {
                                                new StoresFunctionTransaction
                                                    {
                                                        Seq = 1,
                                                        TransactionDefinition = TestTransDefs.InspectionToStores,
                                                        TransactionCode = TestTransDefs.InspectionToStores.TransactionCode
                                                    }
                                            }
            };

        public static readonly StoresFunction AdjustLoc =
            new StoresFunction("ADJUST LOC")
                {
                    QuantityRequired = "N",
                    ToLocationRequired = "N",
                    FromLocationRequired = "Y",
                    FunctionType = "A",
                    ManualPickRequired = "X",
                    ToStateRequired = "N",
                    BatchRequired = "N"
            };

        public static readonly StoresFunction SupplierReturn =
            new StoresFunction("SURETURN")
                {
                    FunctionType = "A",
                    ManualPickRequired = "M",
                    ToStateRequired = "Y",
                    ToStockPool = "LINN"
            };

        public static readonly StoresFunction Move =
            new StoresFunction("MOVE")
                {
                    Description = "STOCK MOVE",
                    FunctionType = "A",
                    BatchRequired = "N",
                    ManualPickRequired = "M",
                    ToStateRequired = "O",
                    ToStockPoolRequired = "O",
                    TransactionsTypes = new List<StoresFunctionTransaction>
                                            {
                                                new StoresFunctionTransaction
                                                    {
                                                        FunctionCode = "MOVE",
                                                        TransactionDefinition = TestTransDefs.StoresMove,
                                                        TransactionCode = TestTransDefs.StoresMove.TransactionCode
                                                    }
                                            }
                };

        public static readonly StoresFunction MoveLocation =
            new StoresFunction("MOVELOC")
            {
                Description = "MOVE ALL STOCK FROM ONE STORAGE PLACE TO ANOTHER",
                FunctionType = "A",
                ToLocationRequired = "N",
                FromLocationRequired = "Y",
                BatchRequired = "N",
                DepartmentNominalRequired = "N",
                LinesRequired = "N",
                ManualPickRequired = "X",
                PartSource = "N",
                ToStateRequired = "N",
                ToStockPoolRequired = "O",
                TransactionsTypes = new List<StoresFunctionTransaction>
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "MOVELOC",
                        TransactionDefinition = TestTransDefs.StoresMove,
                        TransactionCode = TestTransDefs.StoresMove.TransactionCode
                    }
                }
            };

        public static readonly StoresFunction StockToSupplier =
            new StoresFunction("SUREQ")
            {
                Description = "BOOK PARTS FROM STOCK TO A SUPPLIER WITH A REQ",
                FunctionType = "M",
                ToLocationRequired = "Y",
                FromLocationRequired = "O",
                BatchRequired = "N",
                DepartmentNominalRequired = "N",
                LinesRequired = "Y",
                ManualPickRequired = "A",
                PartSource = "N",
                ToStateRequired = "N",
                ToStockPoolRequired = "O",
                TransactionsTypes = new List<StoresFunctionTransaction>
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "SUREQ",
                        TransactionDefinition = TestTransDefs.StockToSupplier,
                        TransactionCode = TestTransDefs.StockToSupplier.TransactionCode
                    }
                }
            };

        public static readonly StoresFunction PartNumberChange =
            new StoresFunction("PARTNO CH")
            {
                Description = "ADJUST PART NUMBER DOWN AND NEW PART NUMBER UP",
                FunctionType = "A",
                ToLocationRequired = "O",
                FromLocationRequired = "O",
                BatchRequired = "N",
                DepartmentNominalRequired = "Y",
                LinesRequired = "N",
                ManualPickRequired = "M",
                PartSource = "IP",
                ToStateRequired = "O",
                ToStockPoolRequired = "O",
                TransactionsTypes = new List<StoresFunctionTransaction>
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "PARTNO CH",
                        TransactionDefinition = TestTransDefs.StockToAdjust,
                        TransactionCode = TestTransDefs.StockToAdjust.TransactionCode,
                        Seq = 1
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "PARTNO CH",
                        TransactionDefinition = TestTransDefs.QCToAdjust,
                        TransactionCode = TestTransDefs.QCToAdjust.TransactionCode,
                        Seq = 2
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "PARTNO CH",
                        TransactionDefinition = TestTransDefs.AdjustToQC,
                        TransactionCode = TestTransDefs.AdjustToQC.TransactionCode,
                        Seq = 3
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "PARTNO CH",
                        TransactionDefinition = TestTransDefs.AdjustToStock,
                        TransactionCode = TestTransDefs.AdjustToStock.TransactionCode,
                        Seq = 4
                    }
                }
            };
    }
}
