namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Resources.Requisitions;

    public class SundryBookInDetailResourceBuilder : IBuilder<SundryBookInDetail>
    {
        public SundryBookInDetailResource Build(SundryBookInDetail sundryBookInDetail, IEnumerable<string> claims)
        {
            if (sundryBookInDetail == null)
            {
                return null;
            }

            return new SundryBookInDetailResource
                       {
                           OrderNumber = sundryBookInDetail.OrderNumber,
                           OrderLine = sundryBookInDetail.OrderLine,
                           Quantity = sundryBookInDetail.Quantity,
                           ReqNumber = sundryBookInDetail.ReqNumber,
                           LineNumber = sundryBookInDetail.LineNumber,
                           TransactionReference = sundryBookInDetail.TransactionReference,
                           DepartmentCode = sundryBookInDetail.DepartmentCode,
                           NominalCode = sundryBookInDetail.NominalCode
                       };
        }

        public string GetLocation(SundryBookInDetail model)
        {
            throw new NotImplementedException();
        }

        object IBuilder<SundryBookInDetail>.Build(SundryBookInDetail entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);
    }
}
