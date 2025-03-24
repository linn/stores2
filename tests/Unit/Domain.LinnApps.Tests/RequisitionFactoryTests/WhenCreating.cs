namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionFactoryTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private RequisitionHeader result;

        private ICreationStrategy cleverStrategy;

        private StoresFunction storesFunction;

        [SetUp]
        public void SetUp()
        {
            this.storesFunction = new StoresFunction("F");
            this.cleverStrategy = Substitute.For<ICreationStrategy>();
            this.StoresFunctionRepository.FindByIdAsync("F").Returns(this.storesFunction);
            this.CreationStrategyResolver
                .Resolve(
                    Arg.Is<RequisitionCreationContext>(
                        r => r.Function.FunctionCode == this.storesFunction.FunctionCode))
                .Returns(this.cleverStrategy);
            this.cleverStrategy.Create(Arg.Any<RequisitionCreationContext>())
                .Returns(new ReqWithReqNumber(1, new Employee(), this.storesFunction, null, 123, null, null, null));
       
            this.result = this.Sut.CreateRequisition(
                123,
                new List<string>(),
                "F",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null).Result;
        }

        [Test]
        public void ShouldGetFunction()
        {
            this.StoresFunctionRepository.Received().FindByIdAsync("F");
        }

        [Test]
        public void ShouldGetStrategy()
        {
            this.CreationStrategyResolver.Received()
                .Resolve(
                    Arg.Is<RequisitionCreationContext>(
                        r => r.Function.FunctionCode == this.storesFunction.FunctionCode));
        }

        [Test]
        public void ShouldExecuteStrategy()
        {
            this.cleverStrategy.Received()
                .Create(
                    Arg.Is<RequisitionCreationContext>(
                        r => r.Function.FunctionCode == this.storesFunction.FunctionCode));
        }

        [Test]
        public void ShouldReturnReq()
        {
            this.result.StoresFunction.FunctionCode.Should().Be(this.storesFunction.FunctionCode);
            this.result.Document1.Should().Be(123);
        }
    }
}
