namespace Linn.Stores2.Domain.LinnApps.Tests.ImportFactoryTests
{
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.PurchaseOrders;
    using Linn.Stores2.Domain.LinnApps.Returns;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IImportFactory Sut { get; set; }

        protected IQueryRepository<Supplier> SupplierRepository { get; set; }

        protected IQueryRepository<Currency> CurrencyRepository { get; set; }

        protected IQueryRepository<Rsn> RsnRepository { get; set; }

        protected IRepository<PurchaseOrder, int> PurchaseOrderRepository { get; set; }

        protected IRepository<ImportBookCpcNumber, int> ImportBookCpcNumberRepository { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.SupplierRepository = Substitute.For<IQueryRepository<Supplier>>();
            this.CurrencyRepository = Substitute.For<IQueryRepository<Currency>>();
            this.RsnRepository = Substitute.For<IQueryRepository<Rsn>>();
            this.PurchaseOrderRepository = Substitute.For<IRepository<PurchaseOrder, int>>();
            this.ImportBookCpcNumberRepository = Substitute.For<IRepository<ImportBookCpcNumber, int>>();

            this.Sut = new ImportFactory(
                this.SupplierRepository,
                this.CurrencyRepository,
                this.RsnRepository,
                this.PurchaseOrderRepository,
                this.ImportBookCpcNumberRepository);
        }
    }
}
