using System;
using DocVelocity.Integration.Encompass.API;
using Monad;

namespace EncompassLoadTest.DataInitialization
{
    public abstract class BaseCreator<TData> : ICreator
    {
        protected readonly IEncompassClient Client;
        protected readonly TData Data;
        protected readonly string ParentId;

        protected BaseCreator(IEncompassClient client, TData data, string parentId)
        {
            Client = client;
            Data = data;
            ParentId = parentId;
        }

        protected void VerifyData()
        {
            if(Data == null)
                throw new ArgumentNullException(nameof(Data));

            if (ParentId == null)
                throw new ArgumentNullException(nameof(ParentId));
        }

        public abstract Try<IResult> Create(string parentId);
    }
}