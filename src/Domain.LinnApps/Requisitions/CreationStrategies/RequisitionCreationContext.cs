namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System.Collections.Generic;

    public class RequisitionCreationContext
    {
        public RequisitionHeader Header { get; set; }

        public int CreatedByUserNumber { get; set; }

        public IEnumerable<string> UserPrivileges { get; set; }

        public LineCandidate FirstLineCandidate { get; set; }
    }
}
