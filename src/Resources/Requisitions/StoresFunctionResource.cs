namespace Linn.Stores2.Resources.Requisitions
{
    using System.Collections.Generic;

    public class StoresFunctionResource
    {
        public string Code { get; set; }

        public string Description { get; set; }

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

        public IEnumerable<FunctionCodeTransactionResource> TransactionTypes { get; set; }
    }
}
