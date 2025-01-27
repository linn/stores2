namespace Linn.Stores2.Domain.LinnApps
{
    public class StoresFunctionCode
    {
        public StoresFunctionCode()
        {
        }

        public StoresFunctionCode(string functionCode)
        {
            this.FunctionCode = functionCode;
        }

        public string FunctionCode { get; set; }
        
        public string Description { get; set; }

        public string CancelFunction { get; set; }
    }
}
