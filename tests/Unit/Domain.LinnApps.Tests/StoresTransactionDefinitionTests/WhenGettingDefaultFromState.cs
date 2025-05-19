namespace Linn.Stores2.Domain.LinnApps.Tests.StoresTransactionDefinitionTests
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using NUnit.Framework;

    public class WhenGettingDefaultFromState
    {
        private StoresTransactionDefinition sut;

        private string result;

        [SetUp]
        public void SetUp()
        {
            this.sut = new StoresTransactionDefinition
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
