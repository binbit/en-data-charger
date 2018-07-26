using System;

namespace EncompassLoadTest.DataInitialization.Errors
{
    public class DocumentError : ResultError
    {
        public string LoanId => ParentId;
        
        public DocumentError(string loanId, Exception exception) : base(loanId, exception)
        {
        }
    }
}