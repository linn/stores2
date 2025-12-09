namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class PartStorageTypeException : DomainException
    {
        public PartStorageTypeException(string message)
            : base(message)
        {
        }

        public PartStorageTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
