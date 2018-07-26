using System;

namespace EncompassLoadTest.DataInitialization.Errors
{
    public abstract class NoError : ResultError
    {
        private NoError(string parentId, Exception exception) : base(parentId, exception)
        {
        }
    }
}