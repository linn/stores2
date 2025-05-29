namespace Linn.Stores2.Domain.LinnApps.External
{
    using System.Threading.Tasks;
    using Linn.Common.Domain;

    public interface ISerialNumberService
    {
        Task<bool> GetSerialNumbersRequired(string partNumber);

        Task<ProcessResult> CheckSerialNumber(string transactionCode, string partNumber, int serialNumber);
    }
}
