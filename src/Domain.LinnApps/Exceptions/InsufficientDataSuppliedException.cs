namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;
     
    public class InsufficientDataSuppliedException : DomainException
    {
        public InsufficientDataSuppliedException(string message)
            : base(message)
        {
        }

        public InsufficientDataSuppliedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
