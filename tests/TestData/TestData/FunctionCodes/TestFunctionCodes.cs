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
                ProcessStage = 1,
                ToStateRequired = "N",
                CanBeReversed = "N",
                UpdateSalesOrderDetQtyOutstanding = "N",
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
                ProcessStage = 1,
                ToStateRequired = "N",
                CanBeReversed = "N",
                UpdateSalesOrderDetQtyOutstanding = "N",
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
                ProcessStage = 1,
                ToStateRequired = "N",
                CanBeReversed = "N",
                UpdateSalesOrderDetQtyOutstanding = "N",
                AuditLocationRequired = "Y"
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
                ProcessStage = 2,
                ToStateRequired = "O",
                CanBeReversed = "Y",
                UpdateSalesOrderDetQtyOutstanding = "N"
            };

        public static readonly StoresFunction BookToLinnDepartment =
            new StoresFunction("BOOKLD")
                {
                    Description = "BOOK IN GOODS FROM SUPPLIER TO LINN DEPARTMENT",
                    BatchRequired = "N",
                    Document1RequiredFlag = "Y",
                    Document1LineRequiredFlag = "N",
                    Document1Text = "Purchase Order",
                    FromStateRequired = "N",
                    FunctionType = "A",
                    ManualPickRequired = "X",
                    PartSource = "PO",
                    ProcessStage = 2,
                    ToStateRequired = "N",
                    CanBeReversed = "Y",
                    UpdateSalesOrderDetQtyOutstanding = "N",
                    ToLocationRequired = "N"
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
                ProcessStage = 2,
                ToStateRequired = "O",
                CanBeReversed = "Y",
                ReceiptDateRequired = "Y",
                DepartmentNominalRequired = "N",
                UpdateSalesOrderDetQtyOutstanding = "N",
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
                ProcessStage = 2,
                CanBeReversed = "Y",
                UpdateSalesOrderDetQtyOutstanding = "N",
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
                ToLocationRequired = "O",
                FromStateRequired = "N",
                FunctionType = "M",
                ManualPickRequired = "A",
                PartSource = "N",
                ProcessStage = 1,
                ToStateRequired = "N",
                ToStockPoolRequired = "O",
                CanBeReversed = "N",
                UpdateSalesOrderDetQtyOutstanding = "N",
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
                ProcessStage = 1,
                ToStateRequired = "N",
                ToStockPoolRequired = "O",
                CanBeReversed = "N",
                UpdateSalesOrderDetQtyOutstanding = "N",
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


        public static readonly StoresFunction LoanBack =
            new StoresFunction("LOAN BACK")
                {
                    Description = "RETURN LOAN GOODS FROM LOAN ACCOUNT TO STORES",
                    BatchRequired = "O",
                    Document1RequiredFlag = "Y",
                    Document1Text = "Loan Number",
                    Document1LineRequiredFlag = "O",
                    DepartmentNominalRequired = "N",
                    FromStateRequired = "N",
                    FunctionType = "A",
                    ManualPickRequired = "X",
                    PartSource = "L",
                    ProcessStage = 1,
                    ToStateRequired = "O",
                    ToStockPoolRequired = "O",
                    CanBeReversed = "Y",
                    UpdateSalesOrderDetQtyOutstanding = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>
                                            {
                                                new StoresFunctionTransaction
                                                    {
                                                        FunctionCode = "LOAN BACK",
                                                        Seq = 1,
                                                        TransactionDefinition = TestTransDefs.LoanToStock,
                                                        TransactionCode = TestTransDefs.LoanToStock.TransactionCode
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
                ProcessStage = 1,
                ToStateRequired = "N",
                CanBeReversed = "N",
                UpdateSalesOrderDetQtyOutstanding = "N",
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
                FromStockPoolRequired = "O",
                FromStateRequired = "N",
                FunctionType = "A",
                ManualPickRequired = "A",
                PartSource = "PO",
                ProcessStage = 1,
                ToLocationRequired = "Y",
                ToStateRequired = "N",
                CanBeReversed = "N",
                UpdateSalesOrderDetQtyOutstanding = "N",
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
                ProcessStage = 2,
                ToStockPoolRequired = "O",
                ToStateRequired = "N",
                CanBeReversed = "N",
                UpdateSalesOrderDetQtyOutstanding = "N",
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
                    ProcessStage = 2,
                    Document1RequiredFlag = "Y",

                    Document1LineRequiredFlag = "O",
                    FunctionType = "A",
                    ToStateRequired = "X",
                    CanBeReversed = "Y",
                    UpdateSalesOrderDetQtyOutstanding = "N",
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

        public static readonly StoresFunction GistReq =
            new StoresFunction("GISTREQ")
                {
                    QuantityRequired = "O",
                    Description = "BOOK UNINSPECTED/FAILED STOCK INTO STORES ON A REQ",
                    BatchRequired = "O",
                    FromStockPoolRequired = "O",
                    ToStockPoolRequired = "O",
                    ManualPickRequired = "M",
                    PartSource = "IP",
                    ProcessStage = 2,
                    Document1RequiredFlag = "N",

                    Document1LineRequiredFlag = "N",
                    FunctionType = "A",
                    ToStateRequired = "X",
                    CanBeReversed = "N",
                    UpdateSalesOrderDetQtyOutstanding = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>
                                            {
                                                new StoresFunctionTransaction
                                                    {
                                                        Seq = 1,
                                                        TransactionDefinition = TestTransDefs.InspectionToStores2,
                                                        TransactionCode = TestTransDefs.InspectionToStores2.TransactionCode
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
                    BatchRequired = "N",
                    ProcessStage = 1,
                    CanBeReversed = "N",
                    UpdateSalesOrderDetQtyOutstanding = "N",
            };

        public static readonly StoresFunction SupplierReturn =
            new StoresFunction("SURETURN")
                {
                    FunctionType = "A",
                    ManualPickRequired = "M",
                    ToStateRequired = "Y",
                    ToStockPool = "LINN",
                    ProcessStage = 1,
                    UpdateSalesOrderDetQtyOutstanding = "N"

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
                    CanBeReversed = "N",
                    ProcessStage = 2,
                    ToLocationRequired = "O",
                    UpdateSalesOrderDetQtyOutstanding = "N",
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
                ProcessStage = 2,
                ToStateRequired = "N",
                ToStockPoolRequired = "O",
                CanBeReversed = "N",
                UpdateSalesOrderDetQtyOutstanding = "N",
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

        public static readonly StoresFunction ReturnToSupplier =
            new StoresFunction("RETSU")
            {
                Description = "RETURN GOODS TO SUPPLIER",
                FunctionType = "A",
                FromLocationRequired = "O",
                FromStateRequired = "Y",
                BatchRequired = "N",
                DepartmentNominalRequired = "N",
                LinesRequired = "N",
                ManualPickRequired = "A",
                PartSource = "RO",
                ProcessStage = 2,
                QuantityRequired = "Y",
                ToLocationRequired = "N",
                ToStateRequired = "N",
                ToStockPoolRequired = "N",
                CanBeReversed = "Y",
                UpdateSalesOrderDetQtyOutstanding = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "RETSU",
                        Seq = 1,
                        TransactionDefinition = TestTransDefs.ReturnGoodsInToInspection,
                        TransactionCode = TestTransDefs.ReturnGoodsInToInspection.TransactionCode
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "RETSU",
                        Seq = 2,
                        TransactionDefinition = TestTransDefs.ReturnStockToSupplier,
                        TransactionCode = TestTransDefs.ReturnStockToSupplier.TransactionCode
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "RETSU",
                        Seq = 3,
                        TransactionDefinition = TestTransDefs.StockToMaterialVarianceReturn,
                        TransactionCode = TestTransDefs.StockToMaterialVarianceReturn.TransactionCode
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "RETSU",
                        Seq = 4,
                        TransactionDefinition = TestTransDefs.MaterialVarianceToStockReturn,
                        TransactionCode = TestTransDefs.MaterialVarianceToStockReturn.TransactionCode
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
                ProcessStage = 1,
                ToStateRequired = "N",
                ToStockPoolRequired = "O",
                CanBeCancelled = "N",
                UpdateSalesOrderDetQtyOutstanding = "N",
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
                ProcessStage = 2,
                ToStateRequired = "O",
                ToStockPoolRequired = "O",
                CanBeCancelled = "N",
                UpdateSalesOrderDetQtyOutstanding = "N",
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

        public static readonly StoresFunction StoresToInspection =
            new StoresFunction("STGII")
            {
                Description = "ISSUE PARTS FROM STORES TO INSPECTION FOR CHECKING",
                BatchRequired = "N",
                DepartmentNominalRequired = "N",
                Document1RequiredFlag = "N",
                Document1LineRequiredFlag = "N",
                FromStockPoolRequired = "O",
                FromStateRequired = "O",
                FunctionType = "A",
                ManualPickRequired = "M",
                PartSource = "IP",
                ProcessStage = 1,
                ToLocationRequired = "O",
                ToStateRequired = "O",
                CanBeReversed = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "STGII",
                        TransactionDefinition = TestTransDefs.StoresToInspection,
                        TransactionCode = TestTransDefs.StoresToInspection.TransactionCode
                    }
                }
            };

        public static readonly StoresFunction OnDem =
            new StoresFunction("ON DEM")
            {
                Description = "MOVE STOCK FROM LINN STORE TO DEMONSTRATION",
                BatchRequired = "N",
                DepartmentNominalRequired = "N",
                Document1RequiredFlag = "N",
                Document1LineRequiredFlag = "N",
                FromStockPoolRequired = "O",
                FromStateRequired = "O",
                FunctionType = "M",
                ManualPickRequired = "M",
                PartSource = "N",
                ProcessStage = 1,
                ToStockPoolRequired = "O",
                ToLocationRequired = "O",
                ToStateRequired = "N",
                CanBeReversed = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "ON DEM",
                        TransactionDefinition = TestTransDefs.MoveToDem,
                        TransactionCode = TestTransDefs.MoveToDem.TransactionCode
                    }
                }
            };

        public static readonly StoresFunction OffDem =
            new StoresFunction("OFF DEM")
            {
                Description = "MOVE DEMONSTRATION STOCK BACK TO LINN",
                BatchRequired = "N",
                DepartmentNominalRequired = "N",
                Document1RequiredFlag = "N",
                Document1LineRequiredFlag = "N",
                FromStockPoolRequired = "O",
                FromStateRequired = "O",
                FunctionType = "M",
                ManualPickRequired = "M",
                PartSource = "N",
                ProcessStage = 1,
                ToStockPoolRequired = "O",
                ToLocationRequired = "O",
                ToStateRequired = "N",
                CanBeReversed = "N",
                UpdateSalesOrderDetQtyOutstanding = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "OFF DEM",
                        TransactionDefinition = TestTransDefs.MoveDemToStock,
                        TransactionCode = TestTransDefs.MoveDemToStock.TransactionCode
                    }
                }
            };

        public static readonly StoresFunction WriteOffFromQC =
            new StoresFunction("WOFF QC")
            {
                Description = "WRITE OFF/ON PARTS IN STOCK",
                BatchRequired = "N",
                DepartmentNominalRequired = "Y",
                Document1RequiredFlag = "N",
                Document1LineRequiredFlag = "N",
                FromStateRequired = "Y",
                FunctionType = "M",
                ManualPickRequired = "M",
                PartSource = "N",
                ProcessStage = 2,
                ToStockPoolRequired = "O",
                ToStateRequired = "N",
                CanBeReversed = "N",
                UpdateSalesOrderDetQtyOutstanding = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "WOFF QC",
                        Seq = 1,
                        TransactionDefinition = TestTransDefs.WriteOffQC,
                        TransactionCode = TestTransDefs.WriteOffQC.TransactionCode,
                        ReqType = "F"
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "WOFF QC",
                        Seq = 2,
                        TransactionDefinition = TestTransDefs.WriteOnQC,
                        TransactionCode = TestTransDefs.WriteOnQC.TransactionCode,
                        ReqType = "O"
                    }
                }
            };
    }
}
