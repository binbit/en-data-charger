using System.Collections.Generic;
using EncompassLoadTest.DataInitialization.Errors;

namespace EncompassLoadTest.DataInitialization.Results
{
    public class InitializationResult : BaseResult<LoanBaseResult, LoanError>
    {
        public string InstanceId => EntityId;
        public IReadOnlyCollection<LoanBaseResult> LoanResults => ResultCollection;
        public IReadOnlyCollection<LoanError> LoanErrors => ErrorCollection;

        public InitializationResult(string instanceId) : base(instanceId)
        {
        }
    }
}