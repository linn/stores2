﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.LdReqCreationStrategyTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenNoLines : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            var context = new RequisitionCreationContext
                              {
                                  UserPrivileges = new List<string>(),
                                  PartNumber = null,
                                  FirstLineCandidate = null
                              };
            this.AuthService.HasPermissionFor(AuthorisedActions.Ldreq, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.action = () => this.Sut.Create(context);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<CreateRequisitionException>()
                .WithMessage(
                    "Cannot create - no lines specified");
        }
    }
}
