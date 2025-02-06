namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenBookingAndDoRequisitionFails : ContextBase
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

            this.AuthService.HasPermissionFor(
                    AuthorisedActions.BookRequisition, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.ReqStoredProcedures.DoRequisition(
                123,
                null,
                user.UserNumber).Returns(new ProcessResult(false, "Stores failure"));

            this.action = async () => await this.Sut.BookRequisition(123, null, user);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<RequisitionException>();
        }
    }
}
