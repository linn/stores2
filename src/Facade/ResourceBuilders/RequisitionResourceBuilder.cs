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
            var reqLineBuilder = new RequisitionLineResourceBuilder();
            var storeFunctionBuilder = new StoresFunctionResourceBuilder();

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
                           Reversed = header.Reversed,
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
                           Links = this.BuildLinks(header, claims).ToArray()
                        };
        }

        public string GetLocation(RequisitionHeader model)
        {
            return $"/requisitions/{model.ReqNumber}";
        }

        object IBuilder<RequisitionHeader>.Build(RequisitionHeader entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(RequisitionHeader model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };

                if (model.Lines != null && model.CanBookReq(null) && this.authService.HasPermissionFor(AuthorisedActions.BookRequisition, claims))
                {
                    yield return new LinkResource { Rel = "book", Href = "/requisitions/book" };
                }

                if (model.RequiresAuthorisation() &&
                    this.authService.HasPermissionFor(model.AuthorisePrivilege(), claims))
                {
                    yield return new LinkResource { Rel = "authorise", Href = "/requisitions/auth" };
                }
            }
        }
    }
}
