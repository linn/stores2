namespace Linn.Stores2.Domain.LinnApps.Tests.QcLabelPrinterServiceTests
{
    using Linn.Common.Domain.LinnApps.Services;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Labels;
    using Linn.Stores2.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IQcLabelPrinterService Sut { get; private set; }

        protected IQueryRepository<LabelType> LabelTypeRepository { get; private set; }

        protected IRepository<Part, string> PartsRepository { get; private set; }

        protected IDocumentProxy DocumentProxy { get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        protected ILabelPrinter LabelPrinter { get; private set; }

        [SetUp]
        public void Setup()
        {
            this.LabelTypeRepository = Substitute.For<IQueryRepository<LabelType>>();
            this.PartsRepository = Substitute.For<IRepository<Part, string>>();
            this.DocumentProxy = Substitute.For<IDocumentProxy>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.LabelPrinter = Substitute.For<ILabelPrinter>();

            this.Sut = new QcLabelPrinterService(
                this.LabelPrinter,
                this.LabelTypeRepository,
                this.PartsRepository,
                this.DocumentProxy,
                this.EmployeeRepository);
        }
    }
}
