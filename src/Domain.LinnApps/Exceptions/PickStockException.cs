namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class PickStockException : DomainException
    {
        public PickStockException(string message)
            : base(message)
        {
        }

        public PickStockException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}