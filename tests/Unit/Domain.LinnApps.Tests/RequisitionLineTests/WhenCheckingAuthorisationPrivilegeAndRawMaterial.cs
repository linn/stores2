namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenCheckingAuthorisationPrivilegeAndRawMaterial : ContextBase
    {
        private string result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.AuthorisePrivilege();
        }

        [Test]
        public void ShouldNotHaveAuthorisePrivilege()
        {
            this.result.Should().BeNull();
        }
    }
}
