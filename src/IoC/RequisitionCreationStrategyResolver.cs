
namespace Linn.Stores2.IoC
{
    using System;

    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using Microsoft.Extensions.DependencyInjection;

    public class RequisitionCreationStrategyResolver : ICreationStrategyResolver
    {
        private readonly IServiceProvider serviceProvider;

        public RequisitionCreationStrategyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider; 
        }

        public ICreationStrategy Resolve(RequisitionCreationContext context)
        {
            if (context.Function.FunctionCode == "LDREQ")
            {
               return this.serviceProvider.GetRequiredService<LdreqCreationStrategy>();
            }
            
            if (context.PartNumber != null && context.Function.FunctionType == "A")
            {
                return this.serviceProvider.GetRequiredService<AutomaticBookFromHeaderStrategy>();
            }

            throw new InvalidOperationException("No strategy found for given scenario");
        }
    }
}
