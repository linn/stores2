namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class PcasStorageTypeException : DomainException
    {
        public PcasStorageTypeException(string message)
            : base(message)
        {
        }

        public PcasStorageTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
