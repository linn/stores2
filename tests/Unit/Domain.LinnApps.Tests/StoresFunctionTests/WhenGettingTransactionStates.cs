namespace Linn.Stores2.Domain.LinnApps.Tests.StoresFunctionTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stores;

    using NUnit.Framework;

    public class WhenGettingTransactionStates
    {
        private StoresFunction sut;

        private IList<string> result;

        [SetUp]
        public void SetUp()
        {
            this.sut = new StoresFunction("ADJUST QC")
            {
                Description = "ADJUST PARTS UP/DOWN IN INSPECTION",
                DepartmentNominalRequired = "Y",
                Document1RequiredFlag = "N",
                FromStateRequired = "Y",
                PartSource = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>
                                        {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "ADJUST QC",
                        Seq = 1,
                        TransactionDefinition = new StoresTransactionDefinition
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
                        },
                        TransactionCode = "ADGII"
                    },
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "ADJUST QC",
                        Seq = 2,
                        TransactionDefinition = new StoresTransactionDefinition
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
                        },
                        TransactionCode = "GIADI"
                    }
                }
            };

            this.result = this.sut.GetTransactionStates("F");
        }

        [Test]
        public void ShouldGetStates()
        {
            this.result.Should().NotBeNull();
            this.result.Count.Should().Be(2);
            this.result.SingleOrDefault(s => s == "QC").Should().NotBeNull();
            this.result.SingleOrDefault(s => s == "FAIL").Should().NotBeNull();
        }
    }
}
