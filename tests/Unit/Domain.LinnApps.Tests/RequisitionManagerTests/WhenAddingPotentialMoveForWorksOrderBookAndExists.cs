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

    public class WhenAddingPotentialMoveForWorksOrderBookAndExists : ContextBase
    {
        private IEnumerable<PotentialMoveDetail> results;

        [SetUp]
        public async Task SetUp()
        {
            this.PotentialMoveDetailRepository
                .FilterByAsync(Arg.Any<Expression<Func<PotentialMoveDetail, bool>>>())
                .Returns(new List<PotentialMoveDetail> { new PotentialMoveDetail { Sequence = 2 } });
            
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
            this.results.First().Sequence.Should().Be(3);
        }

        [Test]
        public void ShouldSavePotentialMove()
        {
            this.PotentialMoveDetailRepository
                .Received()
                .AddAsync(Arg.Is<PotentialMoveDetail>(m => m.DocumentId == 1234));
        }
    }
}
