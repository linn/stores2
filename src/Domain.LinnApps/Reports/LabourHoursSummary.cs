namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System;

    // this class maps onto labour_hours_view used in labour hours reconciliation
    public class LabourHoursSummary
    {
        public DateTime TransactionMonth { get; set; }

        // total from all stock transactions
        // should equal Build - Sold + Other + Back - Out
        public decimal StockTransactions { get; set; }

        public decimal BuildHours { get; set; }

        // this is the way of calculating build hours that works with reconcile
        public decimal AlternativeBuildHours { get; set; }

        // labour hours in sold transactions (incl credits)
        public decimal SoldHours { get; set; }

        public decimal UsedHours { get; set; }

        public decimal LoanOutHours { get; set; }

        public decimal LoanBackHours { get; set; }

        public decimal OtherHours { get; set; }

        public string StartJobref { get; set; }

        // is current month this will be empty, reconcile only works on completed months
        public string EndJobref { get; set; }
    }
}
