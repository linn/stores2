namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Resources.Imports;

    public class ImportBookOrderDetailResourceBuilder : IBuilder<ImportBookOrderDetail>
    {
        public ImportBookOrderDetailResource Build(ImportBookOrderDetail model, IEnumerable<string> claims)
        {
            return new ImportBookOrderDetailResource
                       {
                           ImportBookId = model.ImportBookId,
                           LineNumber = model.LineNumber,
                           OrderNumber = model.OrderNumber,
                           RsnNumber = model.RsnNumber,
                           OrderDescription = model.OrderDescription,
                           Qty = model.Qty,
                           DutyValue = model.DutyValue,
                           FreightValue = model.FreightValue,
                           VatValue = model.VatValue,
                           OrderValue = model.OrderValue,
                           Weight = model.Weight,
                           LoanNumber = model.LoanNumber,
                           LineType = model.LineType,
                           CpcNumberId = model.CpcNumberId,
                           CpcNumber = model.ImportBookCpcNumber?.Description,
                           TariffCode = model.TariffCode,
                           InsNumber = model.InsNumber,
                           VatRate = model.VatRate,
                           PostDuty = model.PostDuty,
                           CountryOfOrigin = model.CountryOfOrigin,
                           LineDocument = model.LineDocument()
                       };
        }

        object IBuilder<ImportBookOrderDetail>.Build(ImportBookOrderDetail model, IEnumerable<string> claims) =>
            this.Build(model, claims);

        public string GetLocation(ImportBookOrderDetail model)
        {
            throw new System.NotImplementedException();
        }
    }
}
