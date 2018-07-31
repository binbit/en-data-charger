namespace EncompassLoadTest.DataInitialization.Results
{
    public class LoanResult : BaseResult
    {
        public string LoanId => EntityId;
        
        public LoanResult(string loanId, string parentId) : base(loanId, parentId)
        {
        }
    }
}