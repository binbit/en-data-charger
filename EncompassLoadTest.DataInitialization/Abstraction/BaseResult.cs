using System;
using System.Collections.Generic;

namespace EncompassLoadTest.DataInitialization
{
    public abstract class BaseResult<TInnerResult, TError> : IResult<TInnerResult, TError>
        where TError : ResultError
    {
        protected string EntityId { get; }

        protected List<TInnerResult> ResultCollection { get; }

        protected List<TError> ErrorCollection { get; }

        protected BaseResult(string entityId)
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