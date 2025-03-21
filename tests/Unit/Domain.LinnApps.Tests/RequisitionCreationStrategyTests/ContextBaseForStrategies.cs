namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests
{
    using Linn.Common.Logging;
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

        protected ILog Log { get; private set; }

        [SetUp]
        public void SetUpContextForAll()
        {
            this.RequisitionManager = Substitute.For<IRequisitionManager>();
            this.RequisitionRepository = Substitute.For<IRepository<RequisitionHeader, int>>();
            this.Log = Substitute.For<ILog>();
        }
    }
}
