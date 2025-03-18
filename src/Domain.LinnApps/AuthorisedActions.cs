namespace Linn.Stores2.Domain.LinnApps
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;

    public class AuthorisedActions
    {
        public const string CancelRequisition = "stores.requisitions.cancel";

        public const string BookRequisition = "stores.requisitions.book";

        public const string Ldreq = "stores.requisitions.LDREQ";

        public const string RequisitionMove = "stores.requisitions.MOVE";

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
