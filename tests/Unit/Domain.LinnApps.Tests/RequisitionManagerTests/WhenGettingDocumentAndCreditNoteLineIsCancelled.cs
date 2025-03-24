namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenGettingDocumentAndCreditNoteLineIsCancelled : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var creditNoteDocument = new DocumentResult("C", 1, 1, 0, "KLYDE");

            this.DocumentProxy.GetCreditNote(1, 1).Returns(creditNoteDocument);

            this.action = () => this.Sut.GetDocument("C", 1, 1);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<DocumentException>()
                .WithMessage("Credit note 1 line 1 is cancelled");
        }
    }
}
