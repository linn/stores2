namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class WorkStationException : DomainException
    {
        public WorkStationException(string message)
            : base(message)
        {
        }

        public WorkStationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
