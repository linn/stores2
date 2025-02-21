namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    public class StoresFunctionTransaction
    {
        public int Seq { get; set; }
        
        public string FunctionCode { get; set; }

        public string TransactionCode { get; set; }

        public StoresTransactionDefinition TransactionDefinition { get; set; }
        
        public string ReqType { get; set; }
    }
}
