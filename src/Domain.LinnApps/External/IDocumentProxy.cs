namespace Linn.Stores2.Domain.LinnApps.External
{
    using System.Threading.Tasks;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public interface IDocumentProxy
    {
        Task<DocumentResult> GetCreditNote(int documentNumber, int? documentLine);

        Task<DocumentResult> GetLoan(int loanNumber);


    }
}
