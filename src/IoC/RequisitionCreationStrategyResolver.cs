namespace Linn.Stores2.IoC
{
    using System;
    using System.Collections.Generic;

    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using Microsoft.Extensions.DependencyInjection;

    public class RequisitionCreationStrategyResolver : ICreationStrategyResolver
    {
        private readonly Dictionary<string, ICreationStrategy> strategies;

        public RequisitionCreationStrategyResolver(IServiceProvider serviceProvider)
        {
            this.strategies = new Dictionary<string, ICreationStrategy>
                                  {
                                      { "LDREQ", serviceProvider.GetRequiredService<LdreqCreationStrategy>() },
                                  };
        }

        public ICreationStrategy Resolve(string functionCode)
        {
            return this.strategies.TryGetValue(functionCode, out var strategy)
                       ? strategy
                       : throw new InvalidOperationException($"No strategy found for function code: {functionCode}");
        }
    }
}
