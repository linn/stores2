namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Resources.Accounts;
    using Linn.Stores2.Resources.Parts;
    using Linn.Stores2.Resources.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private RequisitionHeaderResource resource;

        [SetUp]
        public void Setup()
        {
            this.resource = new RequisitionHeaderResource
                                {
                                    CreatedBy = 33087,
                                    StoresFunction = new StoresFunctionResource { Code = "LDREQ" },
                                    ReqType = "F",
                                    Department = new DepartmentResource { DepartmentCode = "1607" },
                                    Nominal = new NominalResource { NominalCode = "2963" },
                                    Lines = new List<RequisitionLineResource>
                                                {
                                                    new RequisitionLineResource
                                                        {
                                                            LineNumber = 1,
                                                            Part = new PartResource { PartNumber = "PART" },
                                                            Qty = 1,
                                                            TransactionCode = "CODE",
                                                            Moves = new List<MoveResource>
                                                                        {
                                                                            new MoveResource
                                                                                {
                                                                                    Part = "PART",
                                                                                    Qty = 1,
                                                                                    FromLocationCode = "LOC"
                                                                                }
                                                                        }
                                                        }
                                                },
                                    FromStockPool = "LINN"
                                };
            this.RequisitionFactory.CreateRequisition(
                Arg.Any<int>(), 
                Arg.Any<IEnumerable<string>>(),
                this.resource.StoresFunction.Code,
                this.resource.ReqType,
                null, 
                null,
                null,
                this.resource.Department.DepartmentCode,
                this.resource.Nominal.NominalCode,
                Arg.Any<LineCandidate>(),
                reference: null,
                comments: null,
                manualPick: null,
                fromStockPool: this.resource.FromStockPool,
                toStockPool: null,
                fromPalletNumber: null,
                toPalletNumber: null,
                fromLocationCode: null,
                toLocationCode: null,
                partNumber: null,
                quantity: null).Returns(
                new ReqWithReqNumber(
                123,
                new Employee(),
                new StoresFunction
                    {
                        FunctionCode = this.resource.StoresFunction.Code
                    },
                "F",
                123,
                "REQ",
                new Department { DepartmentCode = this.resource.Department.DepartmentCode },
                new Nominal { NominalCode = this.resource.Nominal.NominalCode }));
            this.Response = this.Client.PostAsJsonAsync("/requisitions", this.resource).Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public void ShouldSaveLog()
        {
            this.RequisitionHistoryRepository.Received().AddAsync(Arg.Any<RequisitionHistory>());
        }
    }
}
