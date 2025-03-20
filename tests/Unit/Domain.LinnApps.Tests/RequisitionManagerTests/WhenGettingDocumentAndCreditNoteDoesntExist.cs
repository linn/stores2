namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenGettingDocumentAndCreditNoteDoesntExist : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.DocumentProxy.GetCreditNote(1, 1).Returns((DocumentResult) null);

            this.action = () => this.Sut.GetDocument("C", 1, null);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<DocumentException>()
                .WithMessage("Could not find credit note 1");
        }
    }
}
