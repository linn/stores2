namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionServiceTests
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using FluentAssertions;

    public class WhenAuthorisingAndReqDoesntExist : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            var user = new User
            {
                UserNumber = 33087,
                Privileges = new List<string>()
            };

            this.action = async () => await this.Sut.AuthoriseRequisition(123, user);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<RequisitionException>();
        }
    }
}
