using System;

namespace EncompassLoadTest.DataInitialization
{
    public interface IResult
    {
        string EntityId { get; }
        string ParentEntityId { get; }
        DateTime CreationDate { get; }
        void AddResult(IResult result);
        void AddError(ResultError error);
    }
}