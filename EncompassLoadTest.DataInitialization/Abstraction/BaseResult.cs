using System;
using System.Collections.Generic;

namespace EncompassLoadTest.DataInitialization
{
    public abstract class BaseResult<TResult> : IResult where TResult : IResult
    {
        public string EntityId { get; }
        public string ParentEntityId { get; }

        public IReadOnlyCollection<IResult> Results => ResultCollection;
        public IReadOnlyCollection<ResultError> Errors => ErrorCollection;

        protected List<IResult> ResultCollection { get; }

        protected List<ResultError> ErrorCollection { get; }

        protected BaseResult(string entityId, string parentEntityIdId)
        {
            CreationDate = DateTime.UtcNow;
            EntityId = entityId;
            ParentEntityId = parentEntityIdId;
            ResultCollection = new List<IResult>();
            ErrorCollection = new List<ResultError>();
        }

        public DateTime CreationDate { get; }

        public void AddResult(IResult result)
        {
            ResultCollection.Add(result);
        }

        public void AddError(ResultError error)
        {
            ErrorCollection.Add(error);
        }

        public abstract IEnumerable<TResult> GetInneResults();
    }
}