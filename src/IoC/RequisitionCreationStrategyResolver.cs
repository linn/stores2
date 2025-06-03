namespace Linn.Stores2.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using Microsoft.Extensions.DependencyInjection;

    public class RequisitionCreationStrategyResolver : ICreationStrategyResolver
    {
        private readonly IServiceProvider serviceProvider;

        private List<string> ldreqFunctions = ["LDREQ", "ADJUST", "WOFF", "ADJUST QC", "WOFF QC", "RSN"];

        private List<string> movelocFunctions = ["MOVELOC", "SUREQ"];

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
                return this.serviceProvider.GetRequiredService<AutomaticBookFromHeaderStrategy>();
            }

            if (context.Function.FunctionCode == "GISTREQ")
            {
                if (context.Lines != null && context.Lines.Any())
                {
                    // for now at least. it is technically possible to do a mutli-line GISTREQ
                    // but it hasn't been done since 2007
                    if (context.Lines.Count() > 1)
                    {
                        throw new CreateRequisitionException("Cannot currently GISTREQ more than one line - Speak to IT if you need to do so.");
                    }
                    
                    return this.serviceProvider.GetRequiredService<LinesProvidedStrategy>();
                }
                
                return this.serviceProvider.GetRequiredService<AutomaticBookFromHeaderStrategy>();
            }

            if (context.Function.FunctionCode == "SUREQ")
            {
                return this.serviceProvider.GetRequiredService<SuReqCreationStrategy>();
            }

            if ((context.PartNumber != null || context.Document1Number != null || movelocFunctions.Contains(context.Function.FunctionCode)) && context.Function.AutomaticFunctionType())
            {
                return this.serviceProvider.GetRequiredService<AutomaticBookFromHeaderStrategy>();
            }

            if ((context.PartNumber == null && context.Lines?.Count() > 0) || context.Function.FunctionCode == "AUDIT")
            {
                return this.serviceProvider.GetRequiredService<LinesProvidedStrategy>();
            }

            throw new CreateRequisitionException("No strategy found for given scenario");
        }
    }
}
