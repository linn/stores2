namespace Linn.Stores2.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class CreateNominalPostingException : DomainException
    {
        public CreateNominalPostingException(string message)
            : base(message)
        {
        }

        public CreateNominalPostingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
