namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class StoresPalletException : DomainException
    {
        public StoresPalletException(string message)
            : base(message)
        {
        }

        public StoresPalletException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
