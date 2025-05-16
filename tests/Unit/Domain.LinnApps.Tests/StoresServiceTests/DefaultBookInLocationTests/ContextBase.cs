namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.DefaultBookInLocationTests
{
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NUnit.Framework;

    public class ContextBase : StoresServiceContextBase
    {
        public StorageLocation DefaultForK1 { get; set; }

        public StorageLocation DefaultForK2 { get; set; }

        public StorageLocation LocationResult { get; set; }
        
        [SetUp]
        public void SetUpContext()
        {
            this.DefaultForK1 = new StorageLocation { LocationId = 12, LocationCode = "E-GI-K1" };
            this.DefaultForK2 = new StorageLocation { LocationId = 867, LocationCode = "E-GI-K2" };
        }
    }
}
