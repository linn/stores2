namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentPalletTests
{
    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class ContextBase
    {
        protected ConsignmentPallet Sut { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Sut = new ConsignmentPallet();
        }
    }
}
