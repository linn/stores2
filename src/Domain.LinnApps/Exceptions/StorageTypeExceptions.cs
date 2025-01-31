namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class StorageTypeExceptions : DomainException
    {
        public StorageTypeExceptions(string message)
            : base(message)
        {
        }

        public StorageTypeExceptions(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
