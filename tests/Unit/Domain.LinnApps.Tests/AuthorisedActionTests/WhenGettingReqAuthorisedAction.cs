namespace Linn.Stores2.Domain.LinnApps.Tests.AuthorisedActionTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGettingReqAuthorisedAction
    {
        private string functionCode;

        private string result;

        [SetUp]
        public void SetUp()
        {
            this.functionCode = "MOVELOC";
            this.result = AuthorisedActions.GetRequisitionActionByFunction(this.functionCode);
        }

        [Test]
        public void ShouldReturnAuthString()
        {
            this.result.Should().Be("stores.requisitions.functions.MOVELOC");
        }
    }
}
