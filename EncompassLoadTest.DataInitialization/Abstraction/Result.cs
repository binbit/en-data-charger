using System;
using System.Collections.Generic;

namespace EncompassLoadTest.DataInitialization
{
    public abstract class Result<TInnerResult, TError> : IResult<TInnerResult, TError>
        where TError : ResultError
    {
        protected string EntityId { get; }

        protected List<TInnerResult> ResultCollection { get; }

        protected List<TError> ErrorCollection { get; }

        protected Result(string entityId)
        {
            CreationDate = DateTime.UtcNow;
            EntityId = entityId;
            ResultCollection = new List<TInnerResult>();
            ErrorCollection = new List<TError>();
        }

        public DateTime CreationDate { get; }

        public void AddResult(TInnerResult result)
        {
            ResultCollection.Add(result);
        }

        public void AddError(TError error)
        {
            ErrorCollection.Add(error);
        }
    }
}