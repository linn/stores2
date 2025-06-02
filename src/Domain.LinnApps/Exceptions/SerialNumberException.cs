namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;
    using Linn.Common.Domain.Exceptions;

    public class SerialNumberException : DomainException
    {
        public SerialNumberException(string message) : base(message)
        {
        }

        public SerialNumberException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
