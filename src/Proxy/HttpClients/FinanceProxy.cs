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
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Resources.External;

    public class FinanceProxy : IFinanceProxy
    {
        private readonly IRestClient restClient;

        public FinanceProxy(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public async Task<LedgerPeriodResult> GetLedgerPeriod(string monthName)
        {
            var uri = $"{ConfigurationManager.Configuration["PROXY_ROOT"]}/ledgers/periods?searchTerm={monthName}";

            var response = await this.restClient.Get<IEnumerable<LedgerPeriodResource>>(
                               CancellationToken.None,
                               new Uri(uri, UriKind.RelativeOrAbsolute),
                               new Dictionary<string, string>(),
                               HttpHeaders.AcceptJson);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var detail = response.Value.First();

            return new LedgerPeriodResult
                       {
                           PeriodNumber = detail.PeriodNumber,
                           MonthName = detail.MonthName,
                           StartDate = DateTime.Parse(detail.StartDate),
                           EndDate = DateTime.Parse(detail.EndDate),
                           ReportName = detail.ReportName
                       };
        }

        public async Task<IEnumerable<LedgerPeriodResult>> GetLedgerPeriods()
        {
            var uri = $"{ConfigurationManager.Configuration["PROXY_ROOT"]}/ledgers/periods";

            var response = await this.restClient.Get<IEnumerable<LedgerPeriodResource>>(
                               CancellationToken.None,
                               new Uri(uri, UriKind.RelativeOrAbsolute),
                               new Dictionary<string, string>(),
                               HttpHeaders.AcceptJson);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var details = response.Value;

            return details.Select((LedgerPeriodResource d) => new LedgerPeriodResult
                                                                  {
                                                                      PeriodNumber = d.PeriodNumber,
                                                                      MonthName = d.MonthName,
                                                                      StartDate = DateTime.Parse(d.StartDate),
                                                                      EndDate = DateTime.Parse(d.EndDate),
                                                                      ReportName = d.ReportName
                                                                  });
        }
    }
}
