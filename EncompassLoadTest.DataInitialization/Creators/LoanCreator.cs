using DocVelocity.Integration.Encompass.API;
using Elli.Api.Loans.Model;
using EncompassLoadTest.DataInitialization.Results;
using Monad;

namespace EncompassLoadTest.DataInitialization.Creators
{
    public class LoanCreator : BaseCreator<LoanContract>
    {
        public LoanCreator(IEncompassClient client, LoanContract data, string instanceId) 
            : base(client, data, instanceId)
        {
        }
        
        public override Try<IResult> Create(string parentId)
        {
            VerifyData();

            return () =>
            {
                var loanId = Client.LoanService.CreateLoan(Data);
                return new LoanResult(loanId, parentId);
            };
        }
    }
}