namespace Linn.Stores2.Proxy.External
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

            var response = await this.restClient.Get(CancellationToken.None, new Uri(uri, UriKind.RelativeOrAbsolute), new Dictionary<string, string>(), this.JsonHeaders());

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

        private IDictionary<string, string[]> JsonHeaders()
        {
            return new Dictionary<string, string[]> { { "Accept", new[] { "application/json" } } };
        }
    }
}
