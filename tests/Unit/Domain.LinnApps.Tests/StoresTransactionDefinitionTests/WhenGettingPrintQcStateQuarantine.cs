namespace Linn.Stores2.Domain.LinnApps.Tests.StoresTransactionDefinitionTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NUnit.Framework;

    public class WhenGettingPrintQcStateQuarantine
    {
        private StoresTransactionDefinition sut;

        private string result;

        [SetUp]
        public void SetUp()
        {
            this.sut = new StoresTransactionDefinition { TransactionCode = "SUGII", QcType = "Q" };

            this.result = this.sut.GetPrintQcState();
        }

        [Test]
        public void ShouldNotBePass()
        {
            this.result.Should().Be("QUARANTINE");
        }
    }
}
