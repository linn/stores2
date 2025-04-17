namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidReverseQuantityTests
{
    using NUnit.Framework;

    public class ContextBase : StoresServiceContextBase
    {
        protected int OriginalReqNumber { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.OriginalReqNumber = 456;
        }
    }
}
