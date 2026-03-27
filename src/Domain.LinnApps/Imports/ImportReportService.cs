namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Common.Rendering;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;

    public class ImportReportService : IImportReportService
    {
        private readonly IRepository<ImportBook, int> importBookRepository;

        private readonly ISingleRecordRepository<ImportMaster> importMasterRepository;

        private readonly IQueryRepository<ImportAuthNumber> importAuthNumberRepository;

        private readonly IHtmlTemplateService<ImportClearanceInstruction> clearanceHtmlTemplateService;

        public ImportReportService(IRepository<ImportBook, int> importBookRepository, ISingleRecordRepository<ImportMaster> importMasterRepository, IQueryRepository<ImportAuthNumber> importAuthNumberRepository, IHtmlTemplateService<ImportClearanceInstruction> clearanceHtmlTemplateService)
        {
            this.importBookRepository = importBookRepository;
            this.importMasterRepository = importMasterRepository;
            this.importAuthNumberRepository = importAuthNumberRepository;
            this.clearanceHtmlTemplateService = clearanceHtmlTemplateService;
        }

        public async Task<string> GetClearanceInstructionAsHtml(IEnumerable<int> impbookIds, string toEmailAddress)
        {
            var importMaster = await this.importMasterRepository.GetRecordAsync();

            var model = new ImportClearanceInstruction(importMaster, toEmailAddress);
            var importAuthNumbers = await this.importAuthNumberRepository.FindAllAsync();

            foreach (var id in impbookIds)
            {
                var impbook = await this.importBookRepository.FindByIdAsync(id);
                var matchingAuthNumbers = importAuthNumbers.Where(a => a.Matches(impbook.DateInstructionSent ?? DateTime.UtcNow)).ToList();
                model.AddImportBook(impbook, importAuthNumbers);
            }

            return await this.clearanceHtmlTemplateService.GetHtml(model);
        }
    }
}
