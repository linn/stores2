namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using NUnit.Framework;

    public class WhenValidatingLineCandidateAndNoQty : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.action = () => this.Sut.ValidateLineCandidate(
                new LineCandidate
                {
                    Moves = new[] { new MoveSpecification { Qty = 0 } }
                });
        }

        [Test]
        public async Task ShouldThrowException()
        {
            await this.action.Should().ThrowAsync<RequisitionException>().WithMessage("Move qty is invalid");
        }
    }
}
