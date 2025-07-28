namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class InvalidNominalAccountException : DomainException
    {
        public InvalidNominalAccountException(string message) : base(message)
        {
        }

        public InvalidNominalAccountException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
