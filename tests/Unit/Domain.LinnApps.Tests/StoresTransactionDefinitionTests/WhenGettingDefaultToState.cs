namespace Linn.Stores2.Domain.LinnApps.Tests.StoresTransactionDefinitionTests
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using NUnit.Framework;

    public class WhenGettingDefaultToState
    {
        private StoresTransactionDefinition sut;

        private string result;

        [SetUp]
        public void SetUp()
        {
            this.sut = new StoresTransactionDefinition
            {
                TransactionCode = "STSTM1",
                Description = "MOVE STOCK FROM LINN STORES TO DEMONSTRATION",
                StockAllocations = "Y",
                OntoTransactions = "Y",
                DecrementTransaction = "N",
                TakePriceFrom = "P",
                RequiresAuth = "N",
                FromState = "STORES",
                InspectedState = "STORES",
                StoresTransactionPostings = new List<StoresTransactionPosting>
                {
                    new StoresTransactionPosting("STSTM1", "C", null),
                    new StoresTransactionPosting("STSTM1", "D", null)
                },
                StoresTransactionStates = new List<StoresTransactionState>
                {
                    new StoresTransactionState("O", "STSTM1", "FAIL"),
                    new StoresTransactionState("O", "STSTM1", "STORES"),
                    new StoresTransactionState("F", "STSTM1", "STORES")
                }
            };

            this.result = this.sut.DefaultToState();
        }

        [Test]
        public void ShouldHaveDefaultToState()
        {
            this.result.Should().Be("STORES");
        }
    }
}
