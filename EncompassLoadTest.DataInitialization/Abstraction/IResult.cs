using System;

namespace EncompassLoadTest.DataInitialization
{
    public interface IResult<in TInnerResult, in TError> 
        where TError : ResultError
    {
        DateTime CreationDate { get; }
        void AddResult(TInnerResult result);
        void AddError(TError error);
    }
}