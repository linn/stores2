﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenCreatingLoanReqAndUnsuccessful : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            var processResult = new ProcessResult(false, "Did not make req");

            this.ReqStoredProcedures.CreateLoanReq(100).Returns(processResult);

            this.action = async () => await this.Sut.CreateLoanReq(100);
        }

        [Test]
        public async Task ShouldThrowException()
        {
            await this.action.Should().ThrowAsync<CreateRequisitionException>();
        }
    }
}
