namespace Linn.Stores2.Proxy.HttpClients
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using Linn.Common.Configuration;
    using Linn.Common.Proxy;
    using Linn.Common.Serialization;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Resources.External;

    public class SalesProxy : ISalesProxy
    {
        private readonly IRestClient restClient;

        public SalesProxy(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public async Task<SalesArticleResult> GetSalesArticle(string partNumber)
        {
            var uri = $"{ConfigurationManager.Configuration["PROXY_ROOT"]}/sales/sales-articles/{partNumber}";

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
            var details = json.Deserialize<SalesArticleResource>(response.Value);

            return new SalesArticleResult
                       {
                           ArticleNumber = details.ArticleNumber,
                           InvoiceDescription = details.InvoiceDescription,
                           TypeOfSerialNumber = details.TypeOfSerialNumber,
                           ArticleType = details.ArticleType
                       };
        }
    }
}
