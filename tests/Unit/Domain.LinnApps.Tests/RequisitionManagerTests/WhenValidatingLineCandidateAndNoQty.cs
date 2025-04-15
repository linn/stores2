namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenValidatingLineCandidateAndNoQty : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            var part = new Part { PartNumber = "PART" };
            this.PartRepository.FindByIdAsync(part.PartNumber).Returns(part);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StockToLinnDept.TransactionCode)
                .Returns(TestTransDefs.StockToLinnDept);
            this.action = () => this.Sut.ValidateLineCandidate(
                new LineCandidate
                {
                    Qty = 0,
                    PartNumber = part.PartNumber,
                    TransactionDefinition = TestTransDefs.StockToLinnDept.TransactionCode,
                    Moves = new[] { new MoveSpecification { Qty = 10 } }
                });
        }

        [Test]
        public async Task ShouldThrowException()
        {
            await this.action.Should().ThrowAsync<RequisitionException>()
                .WithMessage("Requisition Line requires a qty");
        }
    }
}
