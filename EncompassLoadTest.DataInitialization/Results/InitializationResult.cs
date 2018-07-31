namespace EncompassLoadTest.DataInitialization.Results
{
    public class InitializationResult : BaseResult
    {
        public string InstanceId => EntityId;

        public InitializationResult(string instanceId) : base(instanceId, string.Empty)
        {
        }
    }
}