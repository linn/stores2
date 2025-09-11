namespace Linn.Stores2.Domain.LinnApps
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;

    public class AuthorisedActions
    {
        public const string CancelRequisition = "stores.requisitions.cancel";

        public const string BookRequisition = "stores.requisitions.book";

        public const string ReverseRequisition = "stores.requisitions.reverse";

        public const string WorkStationAdmin = "workstation.admin";

        public const string StorageLocationAdmin = "stores.storage-locations.admin";

        private const string FunctionsPrefix = "stores.requisitions.functions";

        public static string GetRequisitionActionByFunction(string functionCode)
        {
            if (string.IsNullOrWhiteSpace(functionCode))
            {
                throw new InsufficientDataSuppliedException("No function code supplied");
            }

            return $"{FunctionsPrefix}.{functionCode.ToUpper()}";
        }
    }
}