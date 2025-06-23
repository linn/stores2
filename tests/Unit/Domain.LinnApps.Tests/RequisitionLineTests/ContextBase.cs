namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NUnit.Framework;

    public class ContextBase 
    {
        public RequisitionLine Sut { get; set; }

        public bool BooleanResult { get; set; }

        public ProcessResult ProcessResult { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Sut = new RequisitionLine(
                123,
                1,
                new Part(),
                10,
                new StoresTransactionDefinition());
        }
    }
}
