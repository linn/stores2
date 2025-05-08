using Linn.Common.Facade;

namespace Linn.Stores2.Facade.Services
{
    using System.Threading.Tasks;
    using Linn.Common.Resources;
    using Linn.Stores2.Resources.Requisitions;
    
    public interface IRequisitionLabelsFacadeService
    {
        Task<IResult<ProcessResultResource>> PrintQcLables(QcLabelPrintRequestResource request);
    }
}
