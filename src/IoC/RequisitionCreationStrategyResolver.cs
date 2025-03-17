
using System.Collections.Generic;

namespace Linn.Stores2.IoC
{
    using System;

    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using Microsoft.Extensions.DependencyInjection;

    public class RequisitionCreationStrategyResolver : ICreationStrategyResolver
    {
        private readonly IServiceProvider serviceProvider;

        private List<string> ldreqFunctions = ["LDREQ", "ADJUST", "WOFF", "ADJUST QC", "WOFF QC"];

        public RequisitionCreationStrategyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider; 
        }

        public ICreationStrategy Resolve(RequisitionCreationContext context)
        {
            if (ldreqFunctions.Contains(context.Function.FunctionCode))
            {
                return this.serviceProvider.GetRequiredService<LdreqCreationStrategy>();
            }

            if (context.Function.FunctionCode == "LOAN OUT")
            {
                return this.serviceProvider.GetRequiredService<LoanOutCreationStrategy>();
            }

            if (context.Function.FunctionCode == "GIST PO")
            {
                return this.serviceProvider.GetRequiredService<GistPoCreationStrategy>();
            }

            if (context.PartNumber != null && context.Function.FunctionType == "A")
            {
                return this.serviceProvider.GetRequiredService<AutomaticBookFromHeaderStrategy>();
            }

            throw new InvalidOperationException("No strategy found for given scenario");
        }
    }
}
