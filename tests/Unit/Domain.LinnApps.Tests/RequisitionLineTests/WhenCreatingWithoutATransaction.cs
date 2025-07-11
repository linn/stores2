﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Parts;
    using NUnit.Framework;

    public class WhenCreatingWithoutATransaction : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.action = () => this.Sut = new RequisitionLine(1, 1, TestParts.Cap003, 2, null);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<RequisitionException>()
                .WithMessage("Requisition line requires a transaction");
        }
    }
}
