namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Resources.Accounts;
    using Linn.Stores2.Resources.Parts;
    using Linn.Stores2.Resources.Requisitions;

    using NSubstitute;
    using NSubstitute.ExceptionExtensions;

    using NUnit.Framework;

    public class WhenValidating : ContextBase
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
            this.ReqManager.Validate(
                Arg.Any<int>(),
                this.resource.StoresFunction.Code,
                this.resource.ReqType,
                null,
                null,
                this.resource.Department.DepartmentCode,
                this.resource.Nominal.NominalCode,
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
                quantity: null,
                fromState: null,
                toState: null,
                batchRef: null,
                batchDate: null,
                document1Line: null,
                newPartNumber: null,
                lines: Arg.Any<IEnumerable<LineCandidate>>(),
                isReverseTransaction: null,
                originalDocumentNumber: null)
                .Throws(new CancelRequisitionException("error"));

            this.Response = this.Client.PostAsJsonAsync("/requisitions/validate", this.resource).Result;
        }

        [Test]
        public void ShouldReturnBadRequest()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
