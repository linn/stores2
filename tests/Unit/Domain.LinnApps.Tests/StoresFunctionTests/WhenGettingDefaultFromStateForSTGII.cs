namespace Linn.Stores2.Domain.LinnApps.Tests.StoresFunctionTests
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using NUnit.Framework;

    public class WhenGettingDefaultFromStateForSTGII
    {
        private StoresFunction sut;

        private string result;

        [SetUp]
        public void SetUp()
        {
            this.sut = new StoresFunction("STGII")
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
                        TransactionDefinition = new StoresTransactionDefinition
                                                    {
                                                        TransactionCode = "STGII",
                                                        Description = "ISSUE PARTS FROM STORES TO INSPECTION FOR CHECKING",
                                                        StockAllocations = "N",
                                                        OntoTransactions = "Y",
                                                        DecrementTransaction = "N",
                                                        TakePriceFrom = "P",
                                                        RequiresAuth = "N",
                                                        FromState = "STORES",
                                                        InspectedState = "FAIL",
                                                        StoresTransactionPostings = new List<StoresTransactionPosting>
                                                            {
                                                                new StoresTransactionPosting("STGII", "C", null),
                                                                new StoresTransactionPosting("STGII", "D", null)
                                                            },
                                                        StoresTransactionStates = new List<StoresTransactionState>
                                                            {
                                                                new StoresTransactionState("O", "STGII", "FAIL"),
                                                                new StoresTransactionState("O", "STGII", "QC"),
                                                                new StoresTransactionState("F", "STGII", "STORES"),
                                                            }
                                                    },
                        TransactionCode = "STGII"
                    }
                }
            };

            this.result = this.sut.DefaultFromState();
        }

        [Test]
        public void ShouldHaveDefaultFromState()
        {
            this.result.Should().Be("STORES");
        }
    }
}
