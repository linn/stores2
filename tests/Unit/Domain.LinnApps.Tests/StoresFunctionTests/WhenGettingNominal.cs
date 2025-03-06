namespace Linn.Stores2.Domain.LinnApps.Tests.StoresFunctionTests
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;

    public class WhenGettingNominal
    {
        private StoresFunction sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new StoresFunction("ADJUST")
            {
                Description = "ADJUST PARTS UP/DOWN IN STOCK",
                Document1RequiredFlag = "N",
                PartSource = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>()
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "ADJUST",
                        Seq = 1,
                        TransactionDefinition = new StoresTransactionDefinition
                        {
                            TransactionCode = "ADSTI",
                            Description = "Onto Adjust",
                            OntoTransactions = "Y",
                            DecrementTransaction = "N",
                            TakePriceFrom = "M",
                            RequiresAuth = "N",
                            StoresTransactionPostings = new List<StoresTransactionPosting>
                            {
                                new StoresTransactionPosting("ADSTI", "D", null),
                                new StoresTransactionPosting("ADSTI", "C", new Nominal("0000004710", "STOCK ADJUSTMENTS"))
                            }
                        },
                        TransactionCode = TestTransDefs.StockToAdjust.TransactionCode
                    },
                    new StoresFunctionTransaction
                    {
                        TransactionCode = "STADI",
                        FunctionCode = "ADJUST",
                        Seq = 2,
                        TransactionDefinition = new StoresTransactionDefinition
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
                                new StoresTransactionPosting("STADI", "C", null),
                                new StoresTransactionPosting("STADI", "D", new Nominal("0000004710", "STOCK ADJUSTMENTS"))
                            }
                        }
                    }
                }
            };
        }

        [Test]
        public void ShouldReturnNominal()
        {
            this.sut.GetNominal().Should().NotBeNull();
            this.sut.GetNominal().NominalCode.Should().Be("0000004710");
            this.sut.GetNominal().Description.Should().Be("STOCK ADJUSTMENTS");
        }
    }
}
