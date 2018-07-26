using Elli.Api.Loans.Model;

namespace DocVelocity.Integration.Encompass.API
{
    public interface ILoanService
    {
        LoanContract GetLoan(string id);
        string CreateLoan(LoanContract loan);
        void DeleteLoan(string id);
    }
}