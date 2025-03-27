using System.Threading.Tasks;

namespace Linn.Stores2.Domain.LinnApps.Tests.StockServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using FluentAssertions;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using NSubstitute;
    using NUnit.Framework;
    
    public class WhenCheckingValidStockLocationAndPartNotAtLocation : ContextBase
    {
        private ProcessResult result;
    
        [SetUp]
        public async Task SetUp()
        {
            this.result = await this.Sut.ValidStockLocation(null, 500, "PART", 100m, "QC");
        }

        [Test]
        public void ShouldReturnError()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should().Be("Part PART not at this location");
        }
    }
}
