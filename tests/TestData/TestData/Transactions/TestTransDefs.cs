﻿using Linn.Stores2.Domain.LinnApps.Stores;

namespace Linn.Stores2.TestData.Transactions
{
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public static class TestTransDefs
    {
        public static readonly StoresTransactionDefinition SuppToMatVarTrans =
            new StoresTransactionDefinition
                {
                TransactionCode = "SUMVI",
                Description = "Supplier Material Variance",
                OntoTransactions = "N",
                StockAllocations = "N",
                DecrementTransaction = "N",
                TakePriceFrom = "M",
                RequiresAuth = "N"
            };

        public static readonly StoresTransactionDefinition LinnDeptToStock =
            new StoresTransactionDefinition
                {
                TransactionCode = "LDSTR",
                Description = "Onto Transaction",
                OntoTransactions = "Y",
                DecrementTransaction = "N",
                TakePriceFrom = "M",
                RequiresAuth = "N",
                InspectedState = "STORES",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("LDSTR", "C", null),
                    new StoresTransactionPosting("LDSTR", "D", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("F", "LDSTR", "STORES")
                }
            };

        public static readonly StoresTransactionDefinition StockToLinnDept =
            new StoresTransactionDefinition
                {
                TransactionCode = "STLDI",
                Description = "From Transaction",
                StockAllocations = "Y",
                OntoTransactions = "N",
                DecrementTransaction = "N",
                TakePriceFrom = "P",
                RequiresAuth = "Y",
                AuthOpCode = "AUTH",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("STLDI", "C", null),
                    new StoresTransactionPosting("STLDI", "D", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("F", "LDSTR", "STORES")
                }
            };

        // used in BOOKWO function to decrement stock
        public static readonly StoresTransactionDefinition DecrementToLinnDept =
            new StoresTransactionDefinition
                {
                TransactionCode = "STLDI2",
                Description = "DECREMENT FROM WORKSTATION TO LINN DEPT",
                StockAllocations = "Y",
                OntoTransactions = "N",
                DecrementTransaction = "Y",
                TakePriceFrom = "M",
                RequiresAuth = "N"
            };

        // Generated by BOOKSU function to book material into stock
        public static readonly StoresTransactionDefinition SupplierToStores =
            new StoresTransactionDefinition
                {
                TransactionCode = "SUSTI",
                Description = "SUPPLIER TO STORES GOODS IN",
                StockAllocations = "N",
                OntoTransactions = "Y",
                DecrementTransaction = "N",
                TakePriceFrom = "O",
                RequiresAuth = "N",
                InspectedState = "STORES",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("SUSTI", "C", null),
                    new StoresTransactionPosting("SUSTI", "D", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("O", "SUSTI", "STORES")
                }
            };

        // Generated by BOOKSU function when booking in and when price is less than std price
        public static readonly StoresTransactionDefinition MaterialVarianceBelowStd =
            new StoresTransactionDefinition
                {
                TransactionCode = "SUMVI",
                Description = "BOOK MATERIAL VARIANCE FROM PURCHASE ORDER (LT STD",
                StockAllocations = "N",
                OntoTransactions = "N",
                DecrementTransaction = "N",
                TakePriceFrom = "M",
                RequiresAuth = "N",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("SUMVI", "C", new Nominal("00000012926", "MATERIAL PRICE VARIANCES")),
                    new StoresTransactionPosting("SUMVI", "D", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>()
            };

        // Generated by BOOKSU function to book material into stock in QC state
        public static readonly StoresTransactionDefinition SupplierToQC =
            new StoresTransactionDefinition
            {
                TransactionCode = "SUGII",
                Description = "BOOK GOODS INTO INSPECTION FROM A SUPPLIER",
                StockAllocations = "N",
                OntoTransactions = "Y",
                DecrementTransaction = "N",
                TakePriceFrom = "O",
                RequiresAuth = "N",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("SUGII", "C", null),
                    new StoresTransactionPosting("SUGII", "D", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("O", "SUGII", "QC")
                }
            };

        // Generated by BOOKSU function when booking in and non stock controlled
        public static readonly StoresTransactionDefinition SupplierToNowhere =
            new StoresTransactionDefinition
            {
                TransactionCode = "SUNWI",
                Description = "DECREMENT USED COMPONENTS FROM SUPPLIER STORE",
                StockAllocations = "N",
                OntoTransactions = "N",
                DecrementTransaction = "N",
                TakePriceFrom = "p",
                RequiresAuth = "N",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("SUNWI", "C", null),
                    new StoresTransactionPosting("SUNWI", "D", new Nominal("00000012926", "MATERIAL PRICE VARIANCES"))
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("F", "SUNWI", "QC")
                }
            };

        // Generated by SUKIT function when kitting stock to supplier kit
        public static readonly StoresTransactionDefinition StockToSupplierKit =
            new StoresTransactionDefinition
                {
                TransactionCode = "STSUK",
                Description = "KIT PARTS TO SUPPLIER AGAINST PO",
                StockAllocations = "Y",
                OntoTransactions = "Y",
                DecrementTransaction = "N",
                TakePriceFrom = "P",
                RequiresAuth = "N",
                InspectedState = "LINN"
            };

        // Generated by LOAN OUT when booking out
        public static readonly StoresTransactionDefinition StockToLoan =
            new StoresTransactionDefinition
            {
                TransactionCode = "STLOI",
                Description = "MOVE LOAN PRODUCT FROM STORES TO LOAN ACCOUNT",
                StockAllocations = "Y",
                OntoTransactions = "Y",
                DecrementTransaction = "N",
                TakePriceFrom = "P",
                RequiresAuth = "Y",
                InspectedState = "STORES",
                AuthOpCode = "AUTH"
            };

        // Generated by Adjust
        public static readonly StoresTransactionDefinition AdjustToStock =
            new StoresTransactionDefinition
            {
                TransactionCode = "ADSTI",
                Description = "Onto Adjust",
                OntoTransactions = "Y",
                DecrementTransaction = "N",
                TakePriceFrom = "M",
                RequiresAuth = "N",
                InspectedState = "STORES",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("ADSTI", "C", new Nominal("0000004710", "STOCK ADJUSTMENTS")),
                    new StoresTransactionPosting("ADSTI", "D", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("O", "ADSTI", "STORES")
                }
            };

        public static readonly StoresTransactionDefinition StockToAdjust =
            new StoresTransactionDefinition
            {
                TransactionCode = "STADI",
                Description = "From Adjust",
                StockAllocations = "Y",
                OntoTransactions = "N",
                DecrementTransaction = "N",
                TakePriceFrom = "P",
                RequiresAuth = "Y",
                AuthOpCode = "STORESAUTH",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("STADI", "D", new Nominal("0000004710", "STOCK ADJUSTMENTS")),
                    new StoresTransactionPosting("STADI", "C", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("F", "STADI", "STORES")
                }
            };

        // Generated by Write OFf
        public static readonly StoresTransactionDefinition WriteOn =
            new StoresTransactionDefinition
            {
                TransactionCode = "WOSTI",
                Description = "Write On",
                OntoTransactions = "Y",
                DecrementTransaction = "N",
                TakePriceFrom = "M",
                RequiresAuth = "Y",
                AuthOpCode = "STORESAUTH",
                InspectedState = "LINN",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("WOSTI", "C", new Nominal("0000004729", "STOCK WRITE OFF")),
                    new StoresTransactionPosting("WOSTI", "D", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("O", "WOSTI", "STORES")
                }
            };

        public static readonly StoresTransactionDefinition WriteOff =
            new StoresTransactionDefinition
            {
                TransactionCode = "STWOI",
                Description = "Write Off",
                StockAllocations = "Y",
                OntoTransactions = "N",
                DecrementTransaction = "N",
                TakePriceFrom = "P",
                RequiresAuth = "Y",
                AuthOpCode = "STORESAUTH",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("STWOI", "D", new Nominal("0000004729", "STOCK WRITE OFF")),
                    new StoresTransactionPosting("STWOI", "C", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("F", "STWOI", "STORES")
                }
            };

        // Generated by Adjust QC
        public static readonly StoresTransactionDefinition AdjustToQC =
            new StoresTransactionDefinition
            {
                TransactionCode = "ADGII",
                Description = "Onto Adjust in QC",
                OntoTransactions = "Y",
                DecrementTransaction = "N",
                TakePriceFrom = "M",
                RequiresAuth = "N",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("ADGII", "C", new Nominal("0000004710", "STOCK ADJUSTMENTS")),
                    new StoresTransactionPosting("ADGII", "D", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("O", "ADGII", "FAIL"),
                    new StoresTransactionState("O", "ADGII", "QC")
                }
            };

        public static readonly StoresTransactionDefinition QCToAdjust =
            new StoresTransactionDefinition
            {
                TransactionCode = "GIADI",
                Description = "From Adjust in QC",
                StockAllocations = "Y",
                OntoTransactions = "N",
                DecrementTransaction = "N",
                TakePriceFrom = "P",
                RequiresAuth = "Y",
                AuthOpCode = "STORESAUTH",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("GIADI", "D", new Nominal("0000004710", "STOCK ADJUSTMENTS")),
                    new StoresTransactionPosting("GIADI", "C", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("F", "GIADI", "FAIL"),
                    new StoresTransactionState("F", "GIADI", "QC")
                }
            };

        // Generated by RSN
        public static readonly StoresTransactionDefinition StockToRSN =
            new StoresTransactionDefinition
            {
                TransactionCode = "STRNI",
                Description = "Return parts from RSN to stock",
                OntoTransactions = "Y",
                StockAllocations = "N",
                DecrementTransaction = "N",
                TakePriceFrom = "P",
                RequiresAuth = "Y",
                AuthOpCode = "AUTH",
                DocType = "R",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("STRNI", "C", null),
                    new StoresTransactionPosting("STRNI", "D", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("F", "STRNI", "STORES")
                }
            };

        public static readonly StoresTransactionDefinition RSNToStock =
            new StoresTransactionDefinition
            {
                TransactionCode = "RNSTR",
                Description = "Book out parts from RSN to Stock",
                StockAllocations = "Y",
                OntoTransactions = "N",
                DecrementTransaction = "N",
                TakePriceFrom = "P",
                AuthOpCode = "AUTH",
                InspectedState = "LINN",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("RNSTR", "D", null),
                    new StoresTransactionPosting("RNSTR", "C", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("F", "RNSTR", "STORES"),
                    new StoresTransactionState("O", "RNSTR", "STORES")
                }
            };


        // Generated by CUSTRET
        public static readonly StoresTransactionDefinition CustomerToInspection =
            new StoresTransactionDefinition
            {
                TransactionCode = "CUGIR",
                Description = "Return faulty goods from customer to inspection",
                OntoTransactions = "Y",
                StockAllocations = "N",
                DecrementTransaction = "N",
                TakePriceFrom = "P",
                RequiresAuth = "N",
                DocType = "C",
                Doc2Type = "R",
                InspectedState = "FAIL",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("CUGIR", "C", null),
                    new StoresTransactionPosting("CUGIR", "D", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("O", "CUGIR", "FAIL"),
                    new StoresTransactionState("O", "CUGIR", "QC")
                }
            };

        public static readonly StoresTransactionDefinition CustomerToGoodStock =
            new StoresTransactionDefinition
            {
                TransactionCode = "CUSTR",
                Description = "Return to stock goods returned from customer",
                StockAllocations = "N",
                OntoTransactions = "Y",
                DecrementTransaction = "N",
                TakePriceFrom = "P",
                RequiresAuth = "N",
                DocType = "C",
                InspectedState = "STORES",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("CUSTR", "D", null),
                    new StoresTransactionPosting("CUSTR", "C", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("O", "CUSTR", "STORES")
                }
            };

        public static readonly StoresTransactionDefinition StoresMove =
            new StoresTransactionDefinition
                {
                    TransactionCode = "STSM",
                    Description = "MOVE STOCK"
                };

        public static readonly StoresTransactionDefinition InspectionToStores =
            new StoresTransactionDefinition
                {
                    TransactionCode = "GISTI1",
                    Description = "BOOK PARTS FROM INSPECTION INTO STORES AGAINST PO"
            };

        // Generated by SUREQ function when booking stock to supplier on a req
        public static readonly StoresTransactionDefinition StockToSupplier =
            new StoresTransactionDefinition
            {
                TransactionCode = "STSUI",
                Description = "BOOK PARTS TO A SUPPLIER WITH A REQUISITION",
                StockAllocations = "Y",
                OntoTransactions = "Y",
                DecrementTransaction = "N",
                TakePriceFrom = "P",
                RequiresAuth = "N",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("STSUI", "C", null),
                    new StoresTransactionPosting("STSUI", "D", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("F", "STSUI", "STORES"),
                    new StoresTransactionState("O", "STSUI", "STORES")
                }
            };
    }
}
