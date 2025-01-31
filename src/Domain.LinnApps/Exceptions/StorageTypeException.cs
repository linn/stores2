namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class StorageTypeException : DomainException
    {
        public StorageTypeException(string message)
            : base(message)
        {
        }

        public StorageTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
