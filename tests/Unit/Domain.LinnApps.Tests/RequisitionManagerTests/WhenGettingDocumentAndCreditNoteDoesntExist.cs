namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenGettingDocumentAndCreditNoteDoesntExist : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.DocumentProxy.GetCreditNote(1, 1).Returns((DocumentResult) null);

            this.action = () => this.Sut.GetDocument("C", 1, null);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<DocumentException>()
                .WithMessage("Could not find credit note 1");
        }
    }
}
