namespace Linn.Stores2.Proxy.HttpClients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using Linn.Common.Configuration;
    using Linn.Common.Proxy;
    using Linn.Common.Serialization.Json;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Proxy.External;
    using Linn.Stores2.Resources.External;

    public class DocumentProxy : IDocumentProxy
    {
        private readonly IRestClient restClient;

        public DocumentProxy(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public async Task<DocumentResult> GetCreditNote(int documentNumber, int? documentLine)
        {
            var uri = $"{ConfigurationManager.Configuration["PROXY_ROOT"]}/sales/credit-notes/{documentNumber}";

            var response = await this.restClient.Get(
                               CancellationToken.None, 
                               new Uri(uri, UriKind.RelativeOrAbsolute),
                               new Dictionary<string, string>(),
                               HttpHeaders.AcceptJson);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = new JsonSerializer();
                var creditNote = json.Deserialize<CreditNoteResource>(response.Value);
                if (documentLine == null)
                {
                    return new DocumentResult("C", documentNumber, null, null, null);
                }

                var line = creditNote.Details.SingleOrDefault(l => l.LineNumber == documentLine.Value);
                if (line != null)
                {
                    return new DocumentResult("C", documentNumber, documentLine, line.Quantity, line.ArticleNumber);
                }
            }

            return null; 
        }

        public async Task<DocumentResult> GetLoan(int loanNumber)
        {
            var uri = $"{ConfigurationManager.Configuration["PROXY_ROOT"]}/sales/loans/{loanNumber}";

            var response = await this.restClient.Get(
                               CancellationToken.None,
                               new Uri(uri, UriKind.RelativeOrAbsolute),
                               new Dictionary<string, string>(),
                               HttpHeaders.AcceptJson);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var json = new JsonSerializer();
            var loan = json.Deserialize<DocumentResource>(response.Value);

            return new DocumentResult("L", loan.DocumentNumber, null, null, null);
        }

        public async Task<PurchaseOrderResult> GetPurchaseOrder(int orderNumber)
        {
            var uri = $"{ConfigurationManager.Configuration["PROXY_ROOT"]}/purchasing/purchase-orders/{orderNumber}";

            var response = await this.restClient.Get(
                               CancellationToken.None,
                               new Uri(uri, UriKind.RelativeOrAbsolute),
                               new Dictionary<string, string>(),
                               HttpHeaders.AcceptJson);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var json = new JsonSerializer();
            var po = json.Deserialize<PurchaseOrderResource>(response.Value);

            return new PurchaseOrderResult
                       {
                           OrderNumber = po.OrderNumber,
                           IsAuthorised = po.AuthorisedBy?.Id != null,
                           IsFilCancelled = !string.IsNullOrEmpty(po.DateFilCancelled),
                           DocumentType = po.DocumentType?.Name,
                           Details = po.Details.Select(d => new PurchaseOrderDetailResult
                           {
                               Line = d.Line,
                               OurQty = d.OurQty,
                               PartNumber = d.PartNumber,
                               OriginalOrderNumber = d.OriginalOrderNumber
                           })
                       };
        }

        public async Task<WorksOrderResult> GetWorksOrder(int orderNumber)
        {
            var uri = $"{ConfigurationManager.Configuration["PROXY_ROOT"]}/production/works-orders/{orderNumber}";

            var response = await this.restClient.Get(
                               CancellationToken.None,
                               new Uri(uri, UriKind.RelativeOrAbsolute),
                               new Dictionary<string, string>(),
                               HttpHeaders.AcceptJson);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var json = new JsonSerializer();
            var details = json.Deserialize<WorksOrderResource>(response.Value);

            return new WorksOrderResult
                       {
                           OrderNumber = details.OrderNumber,
                           DateCancelled = details.DateCancelled,
                           PartNumber = details.PartNumber,
                           PartDescription = details.PartDescription,
                           WorkStationCode = details.WorkStationCode,
                           Outstanding = details.Outstanding,
                           Quantity = details.Quantity,
                           QuantityBuilt = details.QuantityBuilt
                       };
        }
    }
}
