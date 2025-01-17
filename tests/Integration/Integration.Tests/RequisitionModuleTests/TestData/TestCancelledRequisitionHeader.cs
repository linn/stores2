namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests.TestData
{
    using System;

    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public class TestCancelledRequisitionHeader : RequisitionHeader
    {
        public TestCancelledRequisitionHeader(int reqNumber)
        {
            this.Cancelled = "Y";
            this.ReqNumber = reqNumber;
            this.DateCancelled = DateTime.Today;
        }
    }
}