namespace Linn.Stores2.Domain.LinnApps.Tests.CreationStategyTests
{
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBaseForStrategies
    {
        protected IRequisitionManager RequisitionManager { get; private set; }
        
        protected IRepository<RequisitionHeader, int> RequisitionRepository { get;  private set; }

        protected RequisitionCreationContext RequisitionCreationContext { get; set; }

        [SetUp]
        public void SetUpContextForAll()
        {
            this.RequisitionManager = Substitute.For<IRequisitionManager>();
            this.RequisitionRepository = Substitute.For<IRepository<RequisitionHeader, int>>();
            this.RequisitionCreationContext = new RequisitionCreationContext();
        }
    }
}
