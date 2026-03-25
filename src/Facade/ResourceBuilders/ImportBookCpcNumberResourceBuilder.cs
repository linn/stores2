namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Resources.Imports;

    public class ImportBookCpcNumberResourceBuilder : IBuilder<ImportBookCpcNumber>
    {
        public ImportBookCpcNumberResource Build(ImportBookCpcNumber model, IEnumerable<string> claims)
        {
            return new ImportBookCpcNumberResource
            {
                CpcNumber = model.CpcNumber,
                Description = model.Description,
                DateInvalid = model.DateInvalid?.ToString("o")
            };
        }

        object IBuilder<ImportBookCpcNumber>.Build(ImportBookCpcNumber model, IEnumerable<string> claims) =>
            this.Build(model, claims);

        public string GetLocation(ImportBookCpcNumber model)
        {
            throw new System.NotImplementedException();
        }
    }
}
