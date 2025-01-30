namespace Linn.Stores2.Resources.Requisitions
{
    using System.Collections.Generic;

    public class FunctionCodeResource
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public IEnumerable<FunctionCodeTransactionResource> TransactionTypes { get; set; }
    }
}
