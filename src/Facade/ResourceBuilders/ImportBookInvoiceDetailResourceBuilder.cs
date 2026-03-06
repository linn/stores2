namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Resources.Imports;

    public class ImportBookInvoiceDetailResourceBuilder : IBuilder<ImportBookInvoiceDetail>
    {
        public ImportBookInvoiceDetailResource Build(
            ImportBookInvoiceDetail model,
            IEnumerable<string> claims)
        {
            return new ImportBookInvoiceDetailResource
            {
                ImportBookId = model.ImportBookId,
                LineNumber = model.LineNumber,
                InvoiceNumber = model.InvoiceNumber,
                InvoiceValue = model.InvoiceValue
            };
        }

        object IBuilder<ImportBookInvoiceDetail>.Build(ImportBookInvoiceDetail model, IEnumerable<string> claims) =>
            this.Build(model, claims);

        public string GetLocation(ImportBookInvoiceDetail model)
        {
            throw new System.NotImplementedException();
        }
    }
}
