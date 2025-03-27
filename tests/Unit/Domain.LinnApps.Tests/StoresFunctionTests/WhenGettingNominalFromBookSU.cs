namespace Linn.Stores2.Domain.LinnApps.Tests.StoresFunctionTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NUnit.Framework;

    public class WhenGettingNominalFromBookSU
    {
        private StoresFunction sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new StoresFunction("BOOKSU")
            {
                Description = "BOOK IN GOODS FROM SUPPLIER FOR PO",
                Document1RequiredFlag = "Y",
                Document1Text = "Order Number",
                PartSource = "PO",
                TransactionsTypes = new List<StoresFunctionTransaction>
                                        {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "BOOKSU",
                        Seq = 1,
                        TransactionDefinition = new StoresTransactionDefinition
                        {
                            TransactionCode = "SUSTI",
                            Description = "SUPPLIER TO STORES GOODS IN",
                            StockAllocations = "N",
                            OntoTransactions = "Y",
                            DecrementTransaction = "N",
                            TakePriceFrom = "O",
                            RequiresAuth = "N",
                            StoresTransactionPostings = new List<StoresTransactionPosting>
                            {
                                new StoresTransactionPosting("SUSTI", "C", null),
                                new StoresTransactionPosting("SUSTI", "D", null)
                            }
                        },
                        TransactionCode = "SUSTI"
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "BOOKSU",
                        Seq = 2,
                        TransactionDefinition = new StoresTransactionDefinition
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
                                new StoresTransactionPosting("SUSTI", "D", null),
                                new StoresTransactionPosting("SUSTI", "C", new Nominal("00000012926", "MATERIAL PRICE VARIANCES"))
                            }
                        },
                        TransactionCode = "SUMVI"
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "BOOKSU",
                        Seq = 3,
                        TransactionDefinition = new StoresTransactionDefinition
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
                            }
                        },
                        TransactionCode = "SUGII"
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "BOOKSU",
                        Seq = 4,
                        TransactionDefinition = new StoresTransactionDefinition
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
                                new StoresTransactionPosting("SUSTI", "C", null),
                                new StoresTransactionPosting("SUSTI", "D", new Nominal("00000012926", "MATERIAL PRICE VARIANCES"))
                            }
                        },
                        TransactionCode = "SUNWI"
                    }
                }
            };
        }

        [Test]
        public void ShouldNotReturnNominal()
        {
            this.sut.GetNominal().Should().BeNull();
        }
    }
}
