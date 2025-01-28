namespace Linn.Stores2.Resources.Requisitions
{
    using System.Collections.Generic;

    public class FunctionCodeResource
    {
        public string Id { get; set; }

        public string DisplayText { get; set; }

        public IEnumerable<FunctionCodeTransactionResource> TransactionTypes { get; set; }
    }
}
