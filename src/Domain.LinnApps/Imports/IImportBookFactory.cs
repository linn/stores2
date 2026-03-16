namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.Imports.Models;

    public interface IImportBookFactory
    {
        Task<ImportBook> CreateImportBook(ImportSetup setup);
    }
}
