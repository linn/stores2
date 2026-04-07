namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Imports;

    public interface IImportBookFacadeService : IAsyncFacadeService<ImportBook, int, ImportBookResource,
        ImportBookResource, ImportBookSearchResource>
    {
        Task<IResult<ImportBookResource>> InitialiseImportBook(
            string rsnNumbers, string purchaseOrderNumbers, int? supplierId, int? employeeNumber, IEnumerable<string> privileges);
    }
}
