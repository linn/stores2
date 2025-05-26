namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingSundryBookInDetails : ContextBase
    {
        private SundryBookInDetail sundryBookInDetail1;

        private SundryBookInDetail sundryBookInDetail2;

        [SetUp]
        public void SetUp()
        {
            this.sundryBookInDetail1 = new SundryBookInDetail { OrderNumber = 123, OrderLine = 1, ReqNumber = 1 };
            this.sundryBookInDetail2 = new SundryBookInDetail { OrderNumber = 123, OrderLine = 1, ReqNumber = 2 };

            this.SundryBookInDetailRepository.FilterByAsync(Arg.Any<Expression<Func<SundryBookInDetail, bool>>>())
                .Returns(new List<SundryBookInDetail> { this.sundryBookInDetail1, this.sundryBookInDetail2 });

            this.Response = this.Client.Get(
                "/requisitions/sundry-book-ins?orderNumber=123&orderLine=1",
                with =>
                {
                    with.Accept("application/json");
                }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<IEnumerable<SundryBookInDetailResource>>().ToList();
            resource.Should().HaveCount(2);
            resource.Should().Contain(a => a.ReqNumber == 1);
            resource.Should().Contain(a => a.ReqNumber == 2);
        }
    }
}
