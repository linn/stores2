using Linn.Stores2.Domain.LinnApps.Stores;

namespace Linn.Stores2.Domain.LinnApps.Tests.StoresFunctionTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;
    using System.Collections.Generic;

    public class WhenGettingDocument1Name
    {
        private StoresFunction sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new StoresFunction("RSN")
            {
                Description = "BOOKS PARTS TO A RSN",
                Document1RequiredFlag = "Y",
                Document1Text = "RSN Number",
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
                        TransactionDefinition = new StoresTransactionDefinition
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
                        },
                        TransactionCode = "STRNI"
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "RSN",
                        Seq = 2,
                        ReqType = "O",
                        TransactionDefinition = new StoresTransactionDefinition
                        {
                            TransactionCode = "RNSTR",
                            Description = "Book out parts from RSN to Stock",
                            StockAllocations = "Y",
                            OntoTransactions = "N",
                            DecrementTransaction = "N",
                            TakePriceFrom = "P",
                            AuthOpCode = "AUTH",
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
                        },
                        TransactionCode = "RNSTR"
                    }
                }
            };
        }

        [Test]
        public void ShouldReturnCorrectDocument1Name()
        {
            this.sut.Document1Name().Should().Be("R");
        }
    }
}
