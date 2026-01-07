namespace Linn.Stores2.Domain.LinnApps.Tests.DeliveryNoteTests
{
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using NSubstitute;
    using NUnit.Framework;

    public class ContextBase
    {
        protected IDeliveryNoteService Sut { get; private set; }

        protected IRequisitionRepository RequisitionRepository { get; private set; }

        protected ISupplierProxy SupplierProxy { get; private set; }

        [SetUp]
        public void Setup()
        {
            this.RequisitionRepository = Substitute.For<IRequisitionRepository>();
            this.SupplierProxy = Substitute.For<ISupplierProxy>();

            this.Sut = new DeliveryNoteService(
                this.RequisitionRepository,
                this.SupplierProxy);
        }
    }
}
