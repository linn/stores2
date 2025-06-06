namespace Linn.Stores2.Proxy.HttpClients
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Linn.Common.Configuration;
    using Linn.Common.Proxy;
    using Linn.Common.Serialization.Json;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Resources.External;

    public class SupplierProxy : ISupplierProxy
    {
        private readonly IRestClient restClient;

        public SupplierProxy(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public async Task<Address> GetSupplierAddress(int supplierId)
        {
            var supplier = await this.GetSupplier(supplierId);

            if (supplier == null || supplier.OrderAddress == null)
            {
                return null;
            }

            var res = supplier?.OrderAddress;
            var address = new Address(
                res.Addressee,
                res.Line1,
                res.Line2,
                res.Line3,
                res.Line4,
                res.PostCode,
                new Country(res.CountryCode, res.CountryName)) { AddressId = res.AddressId.GetValueOrDefault() };
            return address;
        }

        private async Task<SupplierResource> GetSupplier(int supplierId)
        {
            var uri = $"{ConfigurationManager.Configuration["PROXY_ROOT"]}/purchasing/suppliers/{supplierId}";

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
            var supplier = json.Deserialize<SupplierResource>(response.Value);
            return supplier;
        }
    }
}
