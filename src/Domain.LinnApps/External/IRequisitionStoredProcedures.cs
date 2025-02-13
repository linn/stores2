﻿namespace Linn.Stores2.Domain.LinnApps.External
{
    using System.Threading.Tasks;

    using Linn.Common.Domain;

    public interface IRequisitionStoredProcedures
    {
        Task<ProcessResult> UnallocateRequisition(int reqNumber, int? lineNumber, int cancelledBy);

        Task<ProcessResult> DeleteAllocOntos(int reqNumber, int? lineNumber, int? docNumber, string docType);

        Task<ProcessResult> DoRequisition(int reqNumber, int? lineNumber, int bookedBy);

        Task<ProcessResult> CreateRequisitionLines(int reqNumber, int? serialNumber);
    }
}
