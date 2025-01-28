namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Resources.Requisitions;

    public class StoresFunctionCodeResourceBuilder : IBuilder<StoresFunctionCode>
    {
        public FunctionCodeResource Build(StoresFunctionCode model, IEnumerable<string> claims)
        {
            return new FunctionCodeResource
                       {
                          Id = model.FunctionCode,
                          DisplayText = model.Description
                       };
        }

        public string GetLocation(StoresFunctionCode model)
        {
            throw new System.NotImplementedException();
        }

        object IBuilder<StoresFunctionCode>.Build(StoresFunctionCode model, IEnumerable<string> claims) => this.Build(model, claims);
    }
}
