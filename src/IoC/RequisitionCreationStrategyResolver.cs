
namespace Linn.Stores2.IoC
{
    using System;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using Microsoft.Extensions.DependencyInjection;

    public class RequisitionCreationStrategyResolver : ICreationStrategyResolver
    {
        private readonly IServiceProvider serviceProvider;

        public RequisitionCreationStrategyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ICreationStrategy Resolve(RequisitionHeader header)
        {
            if (header.StoresFunction?.FunctionCode == "LDREQ")
            {
               return this.serviceProvider.GetRequiredService<LdreqCreationStrategy>();
            }
            
            if (header.Part != null && header.StoresFunction?.FunctionCode == "A")
            {
                return this.serviceProvider.GetRequiredService<PlaceholderStrategyForWhenHeaderHasPartNumber>();
            }

            throw new InvalidOperationException($"No strategy found for given scenario");
        }
    }
}
