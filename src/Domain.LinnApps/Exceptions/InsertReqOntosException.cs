namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class InsertReqOntosException : DomainException
    {
        public InsertReqOntosException(string message)
            : base(message)
        {
        }

        public InsertReqOntosException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
