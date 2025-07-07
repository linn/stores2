namespace Linn.Stores2.Domain.LinnApps.Tests.StoresTransactionDefinitionTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NUnit.Framework;

    public class WhenGettingPrintQcStatePass
    {
        private StoresTransactionDefinition sut;

        private string result;

        [SetUp]
        public void SetUp()
        {
            this.sut = new StoresTransactionDefinition { TransactionCode = "SUSTI", QcType = "P" };

            this.result = this.sut.GetPrintQcState();
        }

        [Test]
        public void ShouldBePass()
        {
            this.result.Should().Be("PASS");
        }
    }
}
