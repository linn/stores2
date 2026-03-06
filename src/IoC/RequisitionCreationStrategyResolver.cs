namespace Linn.Stores2.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using Microsoft.Extensions.DependencyInjection;

    public class RequisitionCreationStrategyResolver(IServiceProvider serviceProvider) : ICreationStrategyResolver
    {
        private List<string> ldreqFunctions = ["LDREQ", "ADJUST", "WOFF", "ADJUST QC", "WOFF QC", "RSN"];

        private List<string> movelocFunctions = ["MOVELOC", "SUREQ"];

        public ICreationStrategy Resolve(RequisitionCreationContext context)
        {
            if (this.ldreqFunctions.Contains(context.Function.FunctionCode))
            {
                return serviceProvider.GetRequiredService<LdreqCreationStrategy>();
            }

            if (context.Function.FunctionCode == "LOAN OUT")
            {
                return serviceProvider.GetRequiredService<LoanOutCreationStrategy>();
            }

            if (context.Function.FunctionCode == "GIST PO")
            {
                return serviceProvider.GetRequiredService<AutomaticBookFromHeaderStrategy>();
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

                    return serviceProvider.GetRequiredService<LinesProvidedStrategy>();
                }

                return serviceProvider.GetRequiredService<AutomaticBookFromHeaderStrategy>();
            }

            if (context.Function.FunctionCode == "SUREQ")
            {
                return serviceProvider.GetRequiredService<SuReqCreationStrategy>();
            }

            if ((context.PartNumber != null || context.Document1Number != null || this.movelocFunctions.Contains(context.Function.FunctionCode)) && context.Function.AutomaticFunctionType())
            {
                return serviceProvider.GetRequiredService<AutomaticBookFromHeaderStrategy>();
            }

            if ((context.PartNumber == null && context.Lines?.Count() > 0) || context.Function.FunctionCode == "AUDIT")
            {
                return serviceProvider.GetRequiredService<LinesProvidedStrategy>();
            }

            throw new CreateRequisitionException("No strategy found for given scenario");
        }
    }
}
