using System.Collections.Generic;
using EncompassLoadTest.DataInitialization.Errors;

namespace EncompassLoadTest.DataInitialization
{
    public class LoanBaseResult : BaseResult<DocumentBaseResult, DocumentError>
    {
        public string LoanId => EntityId;
        public IReadOnlyCollection<DocumentBaseResult> DocumentResults => ResultCollection;
        public IReadOnlyCollection<DocumentError> DocumentErrors => ErrorCollection;

        public LoanBaseResult(string loanId) : base(loanId)
        {
        }
    }
}