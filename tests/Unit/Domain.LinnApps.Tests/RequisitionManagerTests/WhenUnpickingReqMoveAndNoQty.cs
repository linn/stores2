﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using NUnit.Framework;

    public class WhenUnpickingReqMoveAndNoQty : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.action = async () => await this.Sut.UnpickRequisitionMove(123, 1, 1, 0, 100, false, new List<string>());
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<RequisitionException>()
                .WithMessage("Must unpick positive qty not 0"); ;
        }
    }
}
