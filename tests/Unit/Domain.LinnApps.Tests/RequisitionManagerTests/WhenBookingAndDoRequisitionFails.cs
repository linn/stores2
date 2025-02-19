namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
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
            this.AuthService.HasPermissionFor(
                    AuthorisedActions.BookRequisition, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.ReqStoredProcedures.DoRequisition(
                123,
                null,
                33087).Returns(new ProcessResult(false, "Stores failure"));

            this.action = async () => await this.Sut.BookRequisition(123, null, 33087, new List<string>());
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<RequisitionException>();
        }
    }
}
