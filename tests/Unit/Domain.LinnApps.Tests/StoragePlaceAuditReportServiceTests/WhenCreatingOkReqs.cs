namespace Linn.Stores2.Domain.LinnApps.Tests.StoragePlaceAuditReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingOkReqs : ContextBase
    {
        private ProcessResult result;

        private int employeeNumber;

        [SetUp]
        public void SetUp()
        {
            this.employeeNumber = 111;

            this.StoragePlaceQueryRepository.FilterBy(Arg.Any<Expression<Func<StoragePlace, bool>>>())
                .Returns(new List<StoragePlace> { new StoragePlace { PalletNumber = 745, Name = "P745" } }.AsQueryable());

            this.StoragePlaceAuditPack.CreateAuditReq("P745", this.employeeNumber, null)
                .Returns("SUCCESS");

            this.result = this.Sut.CreateSuccessAuditReqs(
                this.employeeNumber,
                new List<string> { "P745" },
    null,
                null);
        }

        [Test]
        public void ShouldCallPackage()
        {
            this.StoragePlaceAuditPack.Received().CreateAuditReq("P745", this.employeeNumber, null);
        }

        [Test]
        public void ShouldReturnProcessResult()
        {
            this.result.Success.Should().BeTrue();
        }
    }
}
