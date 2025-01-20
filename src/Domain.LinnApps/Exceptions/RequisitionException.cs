namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class RequisitionException : DomainException
    {
        public RequisitionException(string message)
            : base(message)
        {
        }

        public RequisitionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
