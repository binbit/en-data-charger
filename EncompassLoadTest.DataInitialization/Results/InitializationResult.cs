using System.Collections.Generic;
using EncompassLoadTest.DataInitialization.Errors;

namespace EncompassLoadTest.DataInitialization.Results
{
    public class InitializationResult : Result<LoanResult, LoanError>
    {
        public string InstanceId => EntityId;
        public IReadOnlyCollection<LoanResult> LoanResults => ResultCollection;
        public IReadOnlyCollection<LoanError> LoanErrors => ErrorCollection;

        public InitializationResult(string instanceId) : base(instanceId)
        {
        }
    }
}