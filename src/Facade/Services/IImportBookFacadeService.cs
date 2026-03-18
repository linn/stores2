namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Resources.Imports;
    using Linn.Stores2.Resources.Requisitions;

    public interface IImportBookFacadeService : IAsyncFacadeService<ImportBook, int, ImportBookResource,
        ImportBookResource, ImportBookResource>
    {
        Task<IResult<ImportBookResource>> InitialiseImportBook(
            string rsnNumbers, string purchaseOrderNumbers, int? supplierId, IEnumerable<string> privileges);
    }
}
