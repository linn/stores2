namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class WorkstationException : DomainException
    {
        public WorkstationException(string message)
            : base(message)
        {
        }

        public WorkstationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
