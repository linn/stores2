using System.Threading.Tasks;
using Linn.Stores2.Domain.LinnApps.Exceptions;

namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    public class LoanOutCreationStrategy : ICreationStrategy
    {
        private readonly IRequisitionManager requisitionManager;

        public LoanOutCreationStrategy(IRequisitionManager requisitionManager)
        {
            this.requisitionManager = requisitionManager;
        }

        public Task<RequisitionHeader> Create(RequisitionCreationContext context)
        {
            if (context.Function.FunctionCode != "LOAN OUT")
            {
                throw new CreateRequisitionException("Loan Out Creation Strategy requires a LOAN OUT function");
            }

            if (context.Document1Type != "L")
            {
                throw new CreateRequisitionException("Loan Out function requires a Loan document");
            }

            if (context.Document1Number == null)
            {
                throw new CreateRequisitionException("Loan Out function requires a Loan document number");
            }

            return this.requisitionManager.CreateLoanReq(context.Document1Number.Value);
        }
    }
}
