namespace Linn.Stores2.Domain.LinnApps.Tests.StoresFunctionTests
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using NUnit.Framework;

    public class WhenGettingDefaultToState
    {
        private StoresFunction sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new StoresFunction("CUSTRET")
            {
                Description = "RETURN GOODS FROM CUSTOMER TO STOCK/INSPECTION",
                DepartmentNominalRequired = "N",
                Document1RequiredFlag = "Y",
                Document2RequiredFlag = "Y",
                FromStateRequired = "N",
                ToStateRequired = "Y",
                PartSource = "C",
                TransactionsTypes = new List<StoresFunctionTransaction>
                                        {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "CUSTRET",
                        Seq = 1,
                        TransactionDefinition = new StoresTransactionDefinition
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
                        },
                        TransactionCode = "CUGIR"
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "CUSTRET",
                        Seq = 2,
                        TransactionDefinition = new StoresTransactionDefinition
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
                        },
                        TransactionCode = "CUSTR"
                    }
                }
            };
        }

        [Test]
        public void ShouldHaveDefaultToState()
        {
            this.sut.DefaultToState().Should().Be("STORES");
        }
    }
}
