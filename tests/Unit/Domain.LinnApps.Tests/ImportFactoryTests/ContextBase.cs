namespace Linn.Stores2.Domain.LinnApps.Tests.ImportFactoryTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.Domain.LinnApps.Returns;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IImportFactory Sut { get; set; }

        protected IQueryRepository<Supplier> SupplierRepository { get; set; }

        protected IQueryRepository<Currency> CurrencyRepository { get; set; }

        protected IQueryRepository<Rsn> RsnRepository { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.SupplierRepository = Substitute.For<IQueryRepository<Supplier>>();
            this.CurrencyRepository = Substitute.For<IQueryRepository<Currency>>();
            this.RsnRepository = Substitute.For<IQueryRepository<Rsn>>();

            this.Sut = new ImportFactory(
                this.SupplierRepository,
                this.CurrencyRepository,
                this.RsnRepository);
        }
    }
}
