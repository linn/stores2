namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;

    using NUnit.Framework;

    public class WhenAuthorisingAndReqDoesntExist : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.action = async () => await this.Sut.AuthoriseRequisition(123, 33087, new List<string>());
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<RequisitionException>();
        }
    }
}
