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

        public string CancelFunction { get; set; }

        public string DepartmentNominalRequired { get; set; }

        public string ManualPickRequired { get; set; }

        public ICollection<StoresFunctionTransaction> TransactionsTypes { get; set; }

        public bool AuditFunction() => this.FunctionCode == "AUDIT" || this.FunctionCode == "KOUNT";
    }
}
