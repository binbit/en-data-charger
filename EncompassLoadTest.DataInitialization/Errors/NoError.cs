using System;

namespace EncompassLoadTest.DataInitialization.Errors
{
    public abstract class NoError : ResultError
    {
        private NoError() : base(string.Empty, null)
        {
        }
    }
}