﻿namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System.Threading.Tasks;

    public interface ICreationStrategy
    {
        Task Apply(RequisitionCreationContext context);
    }
}
