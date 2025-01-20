namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;
    
    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancellingAndUnauthorised : ContextBase
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
                AuthorisedActions.CancelRequisition, Arg.Any<IEnumerable<string>>())
                .Returns(false);
            this.action = async () => await this.Sut.Cancel(123, user, "REASON");
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
