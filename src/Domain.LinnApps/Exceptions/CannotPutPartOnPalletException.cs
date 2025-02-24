namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class CannotPutPartOnPalletException : DomainException
    {
        public CannotPutPartOnPalletException(string message)
            : base(message)
        {
        }

        public CannotPutPartOnPalletException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
