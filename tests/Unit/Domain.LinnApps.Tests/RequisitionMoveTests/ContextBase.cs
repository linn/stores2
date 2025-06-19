namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionMoveTests
{
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NUnit.Framework;

    public class ContextBase 
    {
        public ReqMove Sut { get; set; }

        public ProcessResult ProcessResult { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Sut = new ReqMove(1, 1, 1, 1, null, 100, null, "LINN", "STORES", "FREE");
        }
    }
}
