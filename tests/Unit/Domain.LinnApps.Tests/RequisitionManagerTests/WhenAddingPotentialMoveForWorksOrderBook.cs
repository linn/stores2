namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NSubstitute;
    using NUnit.Framework;

    public class WhenAddingPotentialMoveForWorksOrderBook : ContextBase
    {
        private IEnumerable<PotentialMoveDetail> results;

        [SetUp]
        public async Task SetUp()
        {
            this.results = await this.Sut.AddPotentialMoveDetails("WO", 1234, 1, "P1", 888, null, 567);
        }

        [Test]
        public void ShouldCheckForExisting()
        {
            this.PotentialMoveDetailRepository.Received()
                .FilterByAsync(Arg.Any<Expression<Func<PotentialMoveDetail, bool>>>());
        }

        [Test]
        public void ShouldReturnPotentialMove()
        {
            this.results.Should().HaveCount(1);
            this.results.First().PartNumber.Should().Be("P1");
            this.results.First().DocumentId.Should().Be(1234);
            this.results.First().Sequence.Should().Be(1);
            this.results.First().DocumentType.Should().Be("WO");
            this.results.First().Quantity.Should().Be(1);
            this.results.First().BuiltBy.Should().Be(888);
            this.results.First().PalletNumber.Should().Be(567);
        }
    }
}
