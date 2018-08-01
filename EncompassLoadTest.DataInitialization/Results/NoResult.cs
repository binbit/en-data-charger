using System;
using System.Collections.Generic;

namespace EncompassLoadTest.DataInitialization.Results
{
    public abstract class NoResult : IResult
    {
        public string EntityId { get; }
        public string ParentEntityId { get; }
        public DateTime CreationDateUtc { get; }
        public void AddResult(IResult result)
        {
            
        }

        public void AddError(ResultError error)
        {
            
        }

        public IEnumerable<string> GetStringResult()
        {
            return null;
        }
    }
}