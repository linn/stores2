namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System.Threading.Tasks;
    using Linn.Stores2.Domain.LinnApps.Exceptions;

    public class LoanOutCreationStrategy : ICreationStrategy
    {
        private readonly IRequisitionManager requisitionManager;

        public LoanOutCreationStrategy(IRequisitionManager requisitionManager)
        {
            this.requisitionManager = requisitionManager;
        }

        public async Task<RequisitionHeader> Create(RequisitionCreationContext context)
        {
            var req = new RequisitionHeader(
                new Employee(),
                context.Function,
                null,
                context.Document1Number,
                context.Document1Type,
                null,
                null);

            if (context.ValidateOnly.GetValueOrDefault())
            {
                return req;
            }

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

            req = await this.requisitionManager.CreateLoanReq(req.Document1.GetValueOrDefault());
            
            // need to add some extra fields to make picking and booking possible
            // REQ_UT does this from function code WVI but we know what they should be
            req.SetStateAndCategory("STORES", "STORES", "FREE");
            req.ReqSource = "STORES2";

            return req;
        }
    }
}
