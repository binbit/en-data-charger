using System;
using System.Collections.Generic;

namespace EncompassLoadTest.DataInitialization
{
    public abstract class BaseResult : IResult
    {
        protected string EntityId { get; }

        protected List<IResult> ResultCollection { get; }

        protected List<ResultError> ErrorCollection { get; }

        protected BaseResult(string entityId)
        {
            CreationDate = DateTime.UtcNow;
            EntityId = entityId;
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
    }
}