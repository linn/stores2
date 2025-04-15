namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidPartNumberChangeTests
{
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Parts;

    public class ContextBase : StoresServiceContextBase
    {
        public ProcessResult result;

        protected Part NewPart { get; set; }
    }
}
