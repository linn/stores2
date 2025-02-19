namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class CancelRequisitionException : DomainException
    {
        public CancelRequisitionException(string message)
            : base(message)
        {
        }

        public CancelRequisitionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
