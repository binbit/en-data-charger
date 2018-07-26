using System;

namespace EncompassLoadTest.DataInitialization.Errors
{
    public class LoanError : ResultError
    {
        public string InstanceId => ParentId;

        public LoanError(string instanceId, Exception exception) : base(instanceId, exception)
        {
        }
    }
}