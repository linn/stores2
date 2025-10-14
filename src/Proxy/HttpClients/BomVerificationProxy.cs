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
    using Linn.Common.Serialization;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Resources.External;

    public class BomVerificationProxy : IBomVerificationProxy
    {
        private readonly IRestClient restClient;

        public BomVerificationProxy(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public async Task<IList<BomVerificationHistory>> GetBomVerifications(string partNumber)
        {
            var uri =
                $"{ConfigurationManager.Configuration["PROXY_ROOT"]}/purchasing/bom-verification?searchTerm={partNumber}";

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
            var resources = json.Deserialize<IEnumerable<BomVerificationHistoryResource>>(response.Value);

            if (resources != null)
            {
                return resources.Select(
                    a => new BomVerificationHistory
                             {
                                 TRef = a.TRef,
                                 PartNumber = a.PartNumber,
                                 VerifiedBy = a.VerifiedBy,
                                 DateVerified = DateTime.Parse(a.DateVerified),
                                 DocumentType = a.DocumentType,
                                 DocumentNumber = a.DocumentNumber,
                                 Remarks = a.Remarks
                             }).ToList();
            }

            return new List<BomVerificationHistory>();
        }
    }
}
