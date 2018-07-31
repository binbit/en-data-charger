using System.Collections.Generic;
using System.Linq;

namespace EncompassLoadTest.DataInitialization.Results
{
    public class InitializationResult : BaseResult<LoanResult>
    {
        public string InstanceId => EntityId;

        public InitializationResult(string instanceId) : base(instanceId, string.Empty)
        {
        }

        public override IEnumerable<LoanResult> GetInneResults()
        {
            return ResultCollection.Cast<LoanResult>();
        }
    }
}