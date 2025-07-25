﻿namespace Linn.Stores2.Domain.LinnApps.External
{
    using System.Threading.Tasks;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public interface IDocumentProxy
    {
        Task<DocumentResult> GetCreditNote(int documentNumber, int? documentLine);

        Task<LoanResult> GetLoan(int loanNumber);

        Task<PurchaseOrderResult> GetPurchaseOrder(int orderNumber);

        Task<WorksOrderResult> GetWorksOrder(int orderNumber);
    }
}
