using System.Collections.Generic;
using EncompassLoadTest.DataInitialization.Errors;

namespace EncompassLoadTest.DataInitialization
{
    public class LoanResult : Result<DocumentResult, DocumentError>
    {
        public string LoanId => EntityId;
        public IReadOnlyCollection<DocumentResult> DocumentResults => ResultCollection;
        public IReadOnlyCollection<DocumentError> DocumentErrors => ErrorCollection;

        public LoanResult(string loanId) : base(loanId)
        {
        }
    }
}