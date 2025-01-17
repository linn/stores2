namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancelling : ContextBase
    {
        private RequisitionHeader req;

        [SetUp]
        public void SetUp()
        {
            this.ReqRepository.FindByIdAsync(123).Returns(new RequisitionHeader(123, "comment"));

            var user = new User
                           {
                               UserNumber = 33087,
                               Privileges = new List<string>()
                           };
            
            this.AuthService.HasPermissionFor(AuthorisedActions.CancelRequisition, Arg.Any<IEnumerable<string>>()).Returns(true);
            this.req = this.Sut.Cancel(123, user).Result;
        }

        [Test]
        public void ShouldReturnCancelled()
        {
            this.req.ReqNumber.Should().Be(123);
        }
    }
}
