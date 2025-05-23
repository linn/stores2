﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.LoanOutCreationStrategyTests
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using NUnit.Framework;

    public class ContextBase : ContextBaseForStrategies
    {
        protected LoanOutCreationStrategy Sut { get; set; }

        protected RequisitionHeader Result { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Sut = new LoanOutCreationStrategy(
                this.RequisitionManager);
        }
    }
}
