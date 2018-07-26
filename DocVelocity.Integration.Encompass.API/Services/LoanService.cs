using Elli.Api.Base;
using Elli.Api.Loans.Api;
using Elli.Api.Loans.Model;
using RestSharp.Extensions;

namespace DocVelocity.Integration.Encompass.API.Services
{
    public class LoanService : ILoanService
    {
        private readonly LoansApi _client;

        public LoanService(AccessToken accessToken)
        {
            _client = ApiClientProvider.GetApiClient<LoansApi>(accessToken);
        }

        public LoanContract GetLoan(string id)
        {
            return _client.GetLoan(id);
        }

        public string CreateLoan(LoanContract loan)
        {
            var response = _client.CreateLoanWithHttpInfo(loanContract: loan, view: "id");
            return response.Headers["Location"].Split('/')[3];
        }

        public void DeleteLoan(string id)
        {
            _client.DeleteLoan(id);
        }
    }
}