namespace Linn.Stores2.Domain.LinnApps.Tests.PackingListServiceTests
{
    using Linn.Common.Pdf;
    using Linn.Common.Persistence;
    using Linn.Common.Rendering;
    using Linn.Stores2.Domain.LinnApps.Consignments;
    using Linn.Stores2.Domain.LinnApps.Consignments.Models;
    using Linn.Stores2.Domain.LinnApps.Consignments.Services;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected PackingListService Sut { get; set; }

        protected IRepository<Consignment, int> ConsignmentRepository { get; private set; }

        protected IPdfService PdfService { get; private set; }

        protected IStringFromFileService StringFromFileService { get; private set; }

        protected IHtmlTemplateService<PackingListDocument> HtmlTemplateService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.ConsignmentRepository = Substitute.For<IRepository<Consignment, int>>();
            this.PdfService = Substitute.For<IPdfService>();
            this.HtmlTemplateService = Substitute.For<IHtmlTemplateService<PackingListDocument>>();
            this.StringFromFileService = Substitute.For<IStringFromFileService>();

            this.Sut = new PackingListService(
                this.ConsignmentRepository,
                this.HtmlTemplateService,
                this.PdfService,
                this.StringFromFileService);
        }
    }
}
