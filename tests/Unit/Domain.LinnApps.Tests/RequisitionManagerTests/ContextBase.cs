namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Domain;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Stores;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IRequisitionManager Sut { get; set; }
        
        protected IAuthorisationService AuthService { get; set; }
        
        protected IRepository<RequisitionHeader, int> ReqRepository { get; set; }

        protected IRepository<Employee, int> EmployeeRepository { get; set; }
        
        protected IRequisitionStoredProcedures ReqStoredProcedures { get; set; }

        protected IRepository<StoresFunction, string> StoresFunctionRepository { get; set; }

        protected IRepository<Department, string> DepartmentRepository { get; set; }

        protected IRepository<Nominal, string> NominalRepository { get; set; }

        protected IRepository<Part, string> PartRepository { get; set; }

        protected IRepository<StoresPallet, int> PalletRepository { get; set; }

        protected IRepository<StockState, string> StateRepository { get; set; }

        protected IRepository<StockPool, string> StockPoolRepository { get; set; }

        protected IRepository<StorageLocation, int> StorageLocationRepository { get; set; }

        protected ITransactionManager TransactionManager { get; set; }

        protected IRepository<StoresTransactionDefinition, string> TransactionDefinitionRepository { get; set; }

        protected IRepository<PotentialMoveDetail, PotentialMoveDetailKey> PotentialMoveDetailRepository { get; set; }

        protected IRepository<BookInOrderDetail, BookInOrderDetailKey> BookInOrderDetailRepository { get; set; }

        protected IQueryRepository<AuditLocation> AuditLocationRepository { get; set; }

        protected IStoresService StoresService { get; private set; }

        protected IDocumentProxy DocumentProxy { get; private set; }

        protected ISalesProxy SalesProxy { get; private set; }

        protected IStockService StockService { get; private set; }

        protected IBomVerificationProxy BomVerificationProxy { get; private set; }

        protected ISerialNumberService SerialNumberService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.ReqRepository = Substitute.For<IRepository<RequisitionHeader, int>>();
            this.ReqStoredProcedures = Substitute.For<IRequisitionStoredProcedures>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.StoresFunctionRepository = Substitute.For<IRepository<StoresFunction, string>>();
            this.DepartmentRepository = Substitute.For<IRepository<Department, string>>();
            this.NominalRepository = Substitute.For<IRepository<Nominal, string>>();
            this.PartRepository = Substitute.For<IRepository<Part, string>>();
            this.StorageLocationRepository = Substitute.For<IRepository<StorageLocation, int>>();
            this.TransactionDefinitionRepository = Substitute.For<IRepository<StoresTransactionDefinition, string>>();
            this.PotentialMoveDetailRepository = Substitute.For<IRepository<PotentialMoveDetail, PotentialMoveDetailKey>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.StoresService = Substitute.For<IStoresService>();
            this.StateRepository = Substitute.For<IRepository<StockState, string>>();
            this.StockPoolRepository = Substitute.For<IRepository<StockPool, string>>();
            this.PalletRepository = Substitute.For<IRepository<StoresPallet, int>>();
            this.DocumentProxy = Substitute.For<IDocumentProxy>();
            this.SalesProxy = Substitute.For<ISalesProxy>();
            this.StockService = Substitute.For<IStockService>();
            this.BomVerificationProxy = Substitute.For<IBomVerificationProxy>();
            this.AuditLocationRepository = Substitute.For<IQueryRepository<AuditLocation>>();
            this.BookInOrderDetailRepository = Substitute.For<IRepository<BookInOrderDetail, BookInOrderDetailKey>>();
            this.SerialNumberService = Substitute.For<ISerialNumberService>();
            this.StoresService.ValidStockPool(Arg.Any<Part>(), Arg.Any<StockPool>())
                .Returns(new ProcessResult(true, "Stock Pool Ok"));
            this.StoresService.ValidState(
                Arg.Any<string>(),
                Arg.Any<StoresFunction>(),
                Arg.Any<string>(),
                Arg.Any<string>())
                .Returns(new ProcessResult(true, "State ok"));
            this.StoresService.ValidDepartmentNominal("0000011111", "0000022222")
                .Returns(new ProcessResult(true, "ok"));
            this.StateRepository.FindByIdAsync("STORES").Returns(new StockState("STORES", "LOVELY STOCK"));

            this.Sut = new RequisitionManager(
                this.AuthService, 
                this.ReqRepository,
                this.ReqStoredProcedures,
                this.EmployeeRepository,
                this.PartRepository,
                this.StorageLocationRepository,
                this.TransactionDefinitionRepository,
                this.TransactionManager,
                this.StoresService,
                this.PalletRepository,
                this.StateRepository,
                this.StockPoolRepository,
                this.StoresFunctionRepository,
                this.DepartmentRepository,
                this.NominalRepository,
                this.DocumentProxy,
                this.StockService,
                this.SalesProxy,
                this.PotentialMoveDetailRepository,
                this.BomVerificationProxy,
                this.BookInOrderDetailRepository,
                this.SerialNumberService,
                this.AuditLocationRepository);
        }
    }
}
