namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Common.Rendering;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;

    public class ImportReportService : IImportReportService
    {
        private readonly IRepository<ImportBook, int> importBookRepository;

        private readonly ISingleRecordRepository<ImportMaster> importMasterRepository;

        private readonly IHtmlTemplateService<ImportClearanceInstruction> clearanceHtmlTemplateService;

        public ImportReportService(IRepository<ImportBook, int> importBookRepository, ISingleRecordRepository<ImportMaster> importMasterRepository, IHtmlTemplateService<ImportClearanceInstruction> clearanceHtmlTemplateService)
        {
            this.importBookRepository = importBookRepository;
            this.importMasterRepository = importMasterRepository;
            this.clearanceHtmlTemplateService = clearanceHtmlTemplateService;
        }

        public async Task<string> GetClearanceInstructionAsHtml(IEnumerable<int> impbookIds, string toEmailAddress)
        {
            var importMaster = await this.importMasterRepository.GetRecordAsync();

            var model = new ImportClearanceInstruction(importMaster, toEmailAddress);

            foreach (var id in impbookIds)
            {
                var impbook = await this.importBookRepository.FindByIdAsync(id);
                model.AddImportBook(impbook);
            }

            return await this.clearanceHtmlTemplateService.GetHtml(model);
        }
    }
}
