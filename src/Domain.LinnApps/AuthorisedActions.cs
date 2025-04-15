namespace Linn.Stores2.Domain.LinnApps
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;

    public class AuthorisedActions
    {
        public const string CancelRequisition = "stores.requisitions.cancel";

        public const string BookRequisition = "stores.requisitions.book";

        public const string ReverseRequisition = "stores.requisitions.reverse";
        
        public static string GetRequisitionActionByFunction(string functionCode)
        {
            if (string.IsNullOrWhiteSpace(functionCode))
            {
                throw new InsufficientDataSuppliedException("No function code supplied");
            }

            return $"stores.requisitions.functions.{functionCode.ToUpper()}";
        }
    }
}
