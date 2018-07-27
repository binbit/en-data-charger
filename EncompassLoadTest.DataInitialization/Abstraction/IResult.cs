using System;

namespace EncompassLoadTest.DataInitialization
{
    public interface IResult
    {
        DateTime CreationDate { get; }
        void AddResult(IResult result);
        void AddError(ResultError error);
    }
}