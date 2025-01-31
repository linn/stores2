namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class PartsStorageTypeException : DomainException
    {
        public PartsStorageTypeException(string message)
            : base(message)
        {
        }

        public PartsStorageTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
