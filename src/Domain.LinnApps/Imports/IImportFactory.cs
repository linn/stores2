namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.Imports.Models;

    public interface IImportFactory
    {
        Task<ImportCandidate> CreateImportBook(
            IEnumerable<int> rsnNumbers,
            IEnumerable<int> poNumbers,
            int? supplierId,
            Employee createdEmployee);
    }
}
