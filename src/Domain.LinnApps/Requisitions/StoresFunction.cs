namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Collections.Generic;

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

        public ICollection<StoresFunctionTransaction> TransactionsTypes { get; set; }

        public bool AuditFunction() => this.FunctionCode == "AUDIT" || this.FunctionCode == "KOUNT";

        public bool Document1Required() => this.Document1RequiredFlag is "Y" or "O" or "X";

        public bool Document1Entered() => this.Document1RequiredFlag is "Y" or "O";
    }
}
