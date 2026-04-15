namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class ContextBase
    {
        protected Consignment Sut { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Sut = new Consignment();
        }
    }
}
