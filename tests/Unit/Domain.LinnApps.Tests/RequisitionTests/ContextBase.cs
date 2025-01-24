namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionTests
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NUnit.Framework;

    public class ContextBase
    {
        protected RequisitionHeader Sut { get; set; }

        protected int ReqNumber { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.ReqNumber = 349378;

            this.Sut = new RequisitionHeader(this.ReqNumber, "Test", new StoresFunctionCode { FunctionCode = "FUNC" });
        }
    }
}
