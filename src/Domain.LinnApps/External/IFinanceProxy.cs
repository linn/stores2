namespace Linn.Stores2.Domain.LinnApps.External
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFinanceProxy
    {
        Task<IEnumerable<LedgerPeriodResult>> GetLedgerPeriods();

        Task<LedgerPeriodResult> GetLedgerPeriod(string monthName);
    }
}
