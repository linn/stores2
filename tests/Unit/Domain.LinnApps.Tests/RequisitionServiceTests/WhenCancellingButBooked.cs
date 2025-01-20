namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;

    using NSubstitute;

    using NUnit.Framework;

    using TestData.Requisitions;

    public class WhenCancellingButBooked : ContextBase
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

            this.ReqRepository.FindByIdAsync(123)
                .Returns(new BookedRequisitionHeader(
                    123, 
                    33087,
                    new StoresFunctionCode { FunctionCode = "F" }));

            this.AuthService.HasPermissionFor(
                AuthorisedActions.CancelRequisition, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.action = async () => await this.Sut.Cancel(123, user, "REASON");
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<RequisitionException>()
                .WithMessage("Cannot cancel a requisition that has already been booked");
        }
    }
}
