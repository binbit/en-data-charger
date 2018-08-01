using System;
using System.Collections.Generic;

namespace EncompassLoadTest.DataInitialization
{
    public interface IResult
    {
        string EntityId { get; }
        string ParentEntityId { get; }
        DateTime CreationDateUtc { get; }
        void AddResult(IResult result);
        void AddError(ResultError error);

        IEnumerable<string> GetStringResult();
    }
}