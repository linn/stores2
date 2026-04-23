namespace Linn.Finance.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class CsvUploadException : DomainException
    {
        public CsvUploadException(string message)
            : base(message)
        {
        }

        public CsvUploadException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
