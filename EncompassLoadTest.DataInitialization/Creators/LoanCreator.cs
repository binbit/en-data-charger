using DocVelocity.Integration.Encompass.API;
using Elli.Api.Loans.Model;

namespace EncompassLoadTest.DataInitialization.Creators
{
    public class LoanCreator : BaseCreator<LoanContract, LoanBaseResult>
    {
        public LoanCreator(IEncompassClient client) : base(client)
        {
        }
        
        public override Try<LoanBaseResult> Create()
        {
            return () =>
            {
                var loanId = Client.LoanService.CreateLoan(Data);
                return new LoanBaseResult(loanId);
            };
        }
    }
}