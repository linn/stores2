namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenGettingDocumentForCreditNote : ContextBase
    {
        private DocumentResult result;

        [SetUp]
        public void SetUp()
        {
            var creditNoteDocument = new DocumentResult("C", 1, 1, 1, "KLYDE");

            this.DocumentProxy.GetCreditNote(1, 1).Returns(creditNoteDocument);

            this.result = this.Sut.GetDocument("C", 1, 1).Result;
        }

        [Test]
        public void ShouldReturnDocument()
        {
            this.result.Should().NotBeNull();
            this.result.DocumentName.Should().Be("C");
            this.result.DocumentNumber.Should().Be(1);
            this.result.LineNumber.Should().Be(1);
            this.result.PartNumber.Should().Be("KLYDE");
            this.result.Quantity.Should().Be(1);
        }
    }
}
