namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Collections.Generic;
    using System.Linq;
    using Linn.Stores2.Domain.LinnApps.Accounts;

    public class StoresFunction
    {
        public StoresFunction()
        {
        }

        public StoresFunction(string functionCode)
        {
            this.FunctionCode = functionCode;
        }

        public string FunctionCode { get; set; }
        
        public string Description { get; set; }

        public string FunctionType { get; set; }

        public string CancelFunction { get; set; }

        public string DepartmentNominalRequired { get; set; }

        public string ManualPickRequired { get; set; }

        public string FromStateRequired { get; set; }

        public string FromLocationRequired { get; set; }

        public string FromStockPoolRequired { get; set; }

        public string QuantityRequired { get; set; }

        public string ToStateRequired { get; set; }
        
        public string ToStockPoolRequired { get; set; }

        public string ToLocationRequired { get; set; }

        // Y - Yes, O, X or N - No
        public string Document1RequiredFlag { get; set; }

        public string Document1Text { get; set; }

        public string PartSource { get; set; }

        public string BatchRequired { get; set; }

        public string BatchDateRequired { get; set; }

        public string FunctionAvailableFlag { get; set; }

        public ICollection<StoresFunctionTransaction> TransactionsTypes { get; set; }

        public bool AuditFunction() => this.FunctionCode == "AUDIT" || this.FunctionCode == "KOUNT";

        public bool Document1Required() => this.Document1RequiredFlag is "Y" or "O" or "X";

        public bool Document1Entered() => this.Document1RequiredFlag is "Y" or "O";

        public bool PartNumberRequired() => this.PartSource != "N";

        public bool FunctionAvailable() => this.FunctionAvailableFlag == "Y";

        public Nominal GetNominal()
        {
            if (this.TransactionsTypes != null)
            {
                return this.TransactionsTypes.FirstOrDefault(t => t.TransactionDefinition?.GetNominal() != null)
                    ?.TransactionDefinition.GetNominal();
            }
            return null;
        }
    }
}
