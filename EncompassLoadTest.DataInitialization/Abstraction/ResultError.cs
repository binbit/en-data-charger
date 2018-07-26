using System;

namespace EncompassLoadTest.DataInitialization
{
    public abstract class ResultError
    {
        public Exception Exception { get; }
        public DateTime OccurDateTime { get; }

        protected readonly string ParentId;

        protected ResultError(string parentId, Exception exception)
        {
            OccurDateTime = DateTime.UtcNow;
            Exception = exception;
            ParentId = parentId;
        }
    }
}