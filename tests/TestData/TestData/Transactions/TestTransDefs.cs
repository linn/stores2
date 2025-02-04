﻿using Linn.Stores2.Domain.LinnApps.Requisitions;

namespace Linn.Stores2.TestData.Transactions
{
    public static class TestTransDefs
    {
        public static readonly StoresTransactionDefinition SuppToMatVarTrans =
            new StoresTransactionDefinition()
            {
                TransactionCode = "SUMVI",
                Description = "Supplier Material Variance",
                OntoTransactions = "N",
                StockAllocations = "N",
                DecrementTransaction = "N",
                TakePriceFrom = "M"
            };

        public static readonly StoresTransactionDefinition LinnDeptToStock =
            new StoresTransactionDefinition()
            {
                TransactionCode = "LDSTR",
                Description = "Onto Transaction",
                OntoTransactions = "Y",
                DecrementTransaction = "N",
                TakePriceFrom = "M"
            };

        public static readonly StoresTransactionDefinition StockToLinnDept =
            new StoresTransactionDefinition()
            {
                TransactionCode = "STLDI",
                Description = "From Transaction",
                StockAllocations = "Y",
                OntoTransactions = "N",
                DecrementTransaction = "N",
                TakePriceFrom = "P"
            };

        // used in BOOKWO function to decrement stock
        public static readonly StoresTransactionDefinition DecrementToLinnDept =
            new StoresTransactionDefinition()
            {
                TransactionCode = "STLDI2",
                Description = "DECREMENT FROM WORKSTATION TO LINN DEPT",
                StockAllocations = "Y",
                OntoTransactions = "N",
                DecrementTransaction = "Y",
                TakePriceFrom = "M"
            };

        // Generated by BOOKSU function to book material into stock
        public static readonly StoresTransactionDefinition SupplierToStores =
            new StoresTransactionDefinition()
            {
                TransactionCode = "SUSTI",
                Description = "SUPPLIER TO STORES GOODS IN",
                StockAllocations = "N",
                OntoTransactions = "Y",
                DecrementTransaction = "N",
                TakePriceFrom = "O"
            };

        // Generated by BOOKSU function when booking in and when price is less than std price
        public static readonly StoresTransactionDefinition MaterialVarianceBelowStd =
            new StoresTransactionDefinition()
            {
                TransactionCode = "SUMVI",
                Description = "BOOK MATERIAL VARIANCE FROM PURCHASE ORDER (LT STD",
                StockAllocations = "N",
                OntoTransactions = "N",
                DecrementTransaction = "N",
                TakePriceFrom = "M"
            };

        // Generated by SUKIT function when kitting stock to supplier kit
        public static readonly StoresTransactionDefinition StockToSupplierKit =
            new StoresTransactionDefinition()
            {
                TransactionCode = "STSUK",
                Description = "KIT PARTS TO SUPPLIER AGAINST PO",
                StockAllocations = "Y",
                OntoTransactions = "Y",
                DecrementTransaction = "N",
                TakePriceFrom = "p"
            };
    }
}
