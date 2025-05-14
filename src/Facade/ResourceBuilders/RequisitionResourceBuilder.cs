namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Resources.Accounts;
    using Linn.Stores2.Resources.Parts;
    using Linn.Stores2.Resources.Requisitions;

    public class RequisitionResourceBuilder : IBuilder<RequisitionHeader>
    {
        private readonly IAuthorisationService authService;

        public RequisitionResourceBuilder(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public RequisitionHeaderResource Build(RequisitionHeader header, IEnumerable<string> claims)
        {
            if (header == null)
            {
                return new RequisitionHeaderResource
                           { 
                               Links = this.BuildLinks(null, claims.ToList()).ToArray()
                           };
            }

            var reqLineBuilder = new RequisitionLineResourceBuilder();
            var storeFunctionBuilder = new StoresFunctionResourceBuilder(this.authService);

            return new RequisitionHeaderResource
                       {
                           ReqNumber = header.ReqNumber, 
                           DateCreated = header.DateCreated.ToString("o"), 
                           Document1 = header.Document1,
                           Quantity = header.Quantity,
                           Document1Name = header.Document1Name,
                           Part = header.Part == null
                                      ? null
                                      : new PartResource
                                            {
                                                PartNumber = header.Part.PartNumber,
                                                Description = header.Part.Description
                                            },        
                           ToLocationId = header.ToLocation?.LocationId,
                           ToLocationCode = header.ToLocation?.LocationCode,
                           FromLocationId = header.FromLocation?.LocationId,
                           FromLocationCode = header.FromLocation?.LocationCode,
                           ToPalletNumber = header.ToPalletNumber,
                           FromPalletNumber = header.FromPalletNumber,
                           Cancelled = header.Cancelled,
                           CancelledBy = header.CancelledBy?.Id,
                           CancelledByName = header.CancelledBy?.Name,
                           DateCancelled = header.DateCancelled?.ToString("o"),
                           CancelledReason = header.CancelledReason,
                           StoresFunction = storeFunctionBuilder.Build(header.StoresFunction, null),
                           Comments = header.Comments,
                           DateBooked = header.DateBooked?.ToString("o"),
                           BookedBy = header.BookedBy?.Id,
                           BookedByName = header.BookedBy?.Name,
                           CreatedBy = header.CreatedBy?.Id,
                           CreatedByName = header.CreatedBy?.Name,
                           IsReversed = header.IsReversed,
                           Lines = header.Lines?.Select(l => reqLineBuilder.Build(l, null)),
                           Nominal = new NominalResource
                                         {
                                             NominalCode = header.Nominal?.NominalCode, 
                                             Description = header.Nominal?.Description
                                         },
                           Department = new DepartmentResource
                                         {
                                             DepartmentCode = header.Department?.DepartmentCode,
                                             Description = header.Department?.Description
                                         },
                           ReqType = header.ReqType,
                           ManualPick = header.ManualPick, 
                           Reference = header.Reference,
                           FromStockPool = header.FromStockPool,
                           ToStockPool = header.ToStockPool,
                           RequiresAuthorisation = header.RequiresAuthorisation(),
                           AuthorisedBy = header.AuthorisedBy?.Id,
                           AuthorisedByName = header.AuthorisedBy?.Name,
                           DateAuthorised = header.DateAuthorised?.ToString("o"),
                           BatchDate = header.BatchDate?.ToString("o"),
                           BatchRef = header.BatchRef,
                           FromState = header.FromState,
                           ToState = header.ToState,    
                           AccountingCompanyCode = header.AccountingCompanyCode(),
                           LoanNumber = header.LoanNumber,
                           Document2 = header.Document2,
                           Document2Name = header.Document2Name,
                           WorkStationCode = header.WorkStationCode,
                           FromCategory = header.FromCategory,
                           ToCategory = header.ToCategory,
                           NewPart = header.NewPart == null
                               ? null
                               : new PartResource
                               {
                                   PartNumber = header.NewPart.PartNumber,
                                   Description = header.NewPart.Description
                               },
                           IsReverseTransaction = header.IsReverseTransaction,
                           OriginalReqNumber = header.OriginalReqNumber,
                           Document3 = header.Document3,
                           Links = this.BuildLinks(header, claims?.ToList()).ToArray()
                       };
        }

        public string GetLocation(RequisitionHeader model)
        {
            return $"/requisitions/{model.ReqNumber}";
        }

        object IBuilder<RequisitionHeader>.Build(RequisitionHeader entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(RequisitionHeader model, IList<string> claims)
        {
            if (claims?.Count > 0)
            {
                yield return new LinkResource { Rel = "create", Href = "/requisitions/create" };
            }
            
            if (this.authService.HasPermissionFor(AuthorisedActions.ReverseRequisition, claims))
            {
                yield return new LinkResource { Rel = "create-reverse", Href = "/requisitions/create" };
            }

            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };

                if (model.Lines != null && model.CanBookReq(null)
                                        && this.authService.HasPermissionFor(AuthorisedActions.BookRequisition, claims))
                {
                    yield return new LinkResource { Rel = "book", Href = "/requisitions/book" };
                }

                if (!model.IsCancelled() && !model.IsBooked()
                                         && this.authService.HasPermissionFor(AuthorisedActions.CancelRequisition, claims))
                {
                    yield return new LinkResource { Rel = "cancel", Href = "/requisitions/cancel" };
                }

                if (model.RequiresAuthorisation() &&
                    this.authService.HasPermissionFor(model.AuthorisePrivilege(), claims))
                {
                    yield return new LinkResource { Rel = "authorise", Href = "/requisitions/auth" };
                }

                if (model.StoresFunction.FunctionCode == "GIST PO")
                {
                    var href = "/requisitions/print-qc-labels?";
                    href += $"&reqNumber={model.ReqNumber}";
                    href += $"&docType={model.Document1Name}";
                    href += $"&orderNumber={model.Document1}";
                    href += "&qcState=PASS";
                    href += $"&partNumber={model.Lines?.First().Part?.PartNumber}";
                    href += $"&partDescription={model.Lines?.First().Part?.Description}";
                    href += $"&qtyReceived={model.Lines?.First().Qty}";
                    href += $"&unitOfMeasure={model.Lines?.First().Part?.OurUnitOfMeasure}";
                    href += $"&qcInfo={model.Lines?.First().Part?.QcInformation}";
                    yield return new LinkResource { Rel = "print-qc-labels", Href = href};
                }

                if (model.HasDeliveryNote())
                {
                    yield return new LinkResource { Rel = "delivery-note", Href = $"/delivery-note/{model.ReqNumber}" };
                }
            }
        }
    }
}
