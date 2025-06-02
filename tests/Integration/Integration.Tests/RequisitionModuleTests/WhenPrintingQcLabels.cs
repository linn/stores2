namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System;
    using System.Net;
    using System.Net.Http.Json;
    using FluentAssertions;
    using Linn.Common.Domain;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Labels;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Requisitions;
    using NSubstitute;
    using NUnit.Framework;
    
    public class WhenPrintingQcLabels : ContextBase
    {
        private QcLabelPrintRequestResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new QcLabelPrintRequestResource
            {
                ReqNumber = 123
            };
            this.QcLabelPrinterService.PrintLabels(Arg.Any<QcLabelPrintRequest>())
                .Returns(new ProcessResult(true, String.Empty));
            this.Response = this.Client.PostAsJsonAsync("/requisitions/print-qc-labels", this.resource).Result;
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
        public void ShouldReturnASuccess()
        {
            var result = this.Response.DeserializeBody<ProcessResultResource>();
            result.Success.Should().BeTrue();
        }
    }
}
