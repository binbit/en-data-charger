using System;
using DocVelocity.Integration.Encompass.API;

namespace EncompassLoadTest.DataInitialization
{
    public abstract class BaseCreator<TData, TResult> : ICreator<TData, TResult>
    {
        protected readonly IEncompassClient Client;
        protected TData Data;
        protected string ParentId;

        protected BaseCreator(IEncompassClient client)
        {
            Client = client;
        }

        protected void VerifyData()
        {
            if(Data == null)
                throw new ArgumentNullException(nameof(Data));

            if (ParentId == null)
                throw new ArgumentNullException(nameof(ParentId));
        }

        public void LoadData(string parentId, TData data)
        {
            Data = data;
            ParentId = parentId;
        }

        public abstract Try<TResult> Create();
    }
}