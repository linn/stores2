namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Resources.Imports;

    public class ImportBookPostEntryResourceBuilder : IBuilder<ImportBookPostEntry>
    {
        public ImportBookPostEntryResource Build(ImportBookPostEntry model, IEnumerable<string> claims)
        {
            return new ImportBookPostEntryResource
                       {
                           ImportBookId = model.ImportBookId,
                           LineNumber = model.LineNumber,
                           EntryCodePrefix = model.EntryCodePrefix,
                           EntryCode = model.EntryCode,
                           EntryDate = model.EntryDate?.ToString("o"),
                           Reference = model.Reference,
                           Duty = model.Duty,
                           Vat = model.Vat
                       };
        }

        object IBuilder<ImportBookPostEntry>.Build(ImportBookPostEntry model, IEnumerable<string> claims) =>
            this.Build(model, claims);

        public string GetLocation(ImportBookPostEntry model)
        {
            throw new System.NotImplementedException();
        }
    }
}
