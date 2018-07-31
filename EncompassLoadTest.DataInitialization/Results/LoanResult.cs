using System.Collections.Generic;
using System.Linq;

namespace EncompassLoadTest.DataInitialization.Results
{
    public class LoanResult : BaseResult<DocumentResult>
    {
        public string LoanId => EntityId;
        
        public LoanResult(string loanId, string parentId) : base(loanId, parentId)
        {
        }

        public override IEnumerable<DocumentResult> GetInneResults()
        {
            return ResultCollection.Cast<DocumentResult>();
        }
    }
}