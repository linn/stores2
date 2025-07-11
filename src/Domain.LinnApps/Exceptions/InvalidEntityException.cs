namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;
    using Common.Domain.Exceptions;

    public class InvalidEntityException : DomainException
    {
        public InvalidEntityException(string message) : base(message)
        {
        }

        public InvalidEntityException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
