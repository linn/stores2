namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using NUnit.Framework;

    public class WhenUnpickingReqMoveAndReqDoesntExist : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.action = async () => await this.Sut.UnpickRequisitionMove(123, 1, 1,1,100, false, new List<string>());
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<RequisitionException>()
                .WithMessage("Req 123 not found"); ;
        }
    }
}
