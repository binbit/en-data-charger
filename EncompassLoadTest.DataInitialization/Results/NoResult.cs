using System;

namespace EncompassLoadTest.DataInitialization.Results
{
    public abstract class NoResult : IResult
    {
        public string EntityId { get; }
        public string ParentEntityId { get; }
        public DateTime CreationDate { get; }
        public void AddResult(IResult result)
        {
            
        }

        public void AddError(ResultError error)
        {
            
        }
    }
}