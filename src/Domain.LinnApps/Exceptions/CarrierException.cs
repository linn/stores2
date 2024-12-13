namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class CarrierException : DomainException
    {
        public CarrierException(string message)
            : base(message)
        {
        }

        public CarrierException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
