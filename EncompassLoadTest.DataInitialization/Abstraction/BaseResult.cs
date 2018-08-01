using System;
using System.Collections.Generic;
using System.Linq;

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
            CreationDateUtc = DateTime.UtcNow;
            EntityId = entityId;
            ParentEntityId = parentEntityIdId;
            ResultCollection = new List<IResult>();
            ErrorCollection = new List<ResultError>();
        }

        public DateTime CreationDateUtc { get; }

        public void AddResult(IResult result)
        {
            ResultCollection.Add(result);
        }

        public void AddError(ResultError error)
        {
            ErrorCollection.Add(error);
        }

        public IEnumerable<string> GetStringResult()
        {
            var resultStrings = new List<string>();
            if (ResultCollection.Any())
            {
                foreach (var result in ResultCollection)
                {
                    foreach (var stringResult in result.GetStringResult())
                    {
                        resultStrings.Add($"{EntityId}|{CreationDateUtc:O}|{stringResult}");
                    }
                }
            }
            else
            {
                return new List<string> {$"{EntityId}|{CreationDateUtc:O}"};
            }

            return resultStrings;
        }
    }
}