namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;

    public class AuditLocationResourceBuilder : IBuilder<AuditLocation>
    {
        public AuditLocationResource Build(AuditLocation model, IEnumerable<string> claims)
        {
            return new AuditLocationResource
            {
                StoragePlace = model.StoragePlace,
                PalletLocationOrArea = model.PalletLocationOrArea
            };
        }

        public string GetLocation(AuditLocation model)
        {
            throw new NotImplementedException();
        }

        object IBuilder<AuditLocation>.Build(AuditLocation model, IEnumerable<string> claims) => this.Build(model, claims);
    }
}
