namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class DocumentException : DomainException
    {
        public DocumentException(string message) : base(message)
        {
        }

        public DocumentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
