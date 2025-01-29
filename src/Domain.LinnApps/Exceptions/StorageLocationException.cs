using System;
using Linn.Common.Domain.Exceptions;

namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    public class StorageLocationException : DomainException
    {
        public StorageLocationException(string message) : base(message)
        {
        }

        public StorageLocationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
