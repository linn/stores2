namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class CreateRequisitionException : DomainException
    {
        public CreateRequisitionException(string message)
            : base(message)
        {
        }

        public CreateRequisitionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}