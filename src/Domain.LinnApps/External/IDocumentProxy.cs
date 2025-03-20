namespace Linn.Stores2.Domain.LinnApps.External
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public interface IDocumentProxy
    {
        DocumentResult  GetCreditNote(int documentNumber, int? documentLine);
    }
}
