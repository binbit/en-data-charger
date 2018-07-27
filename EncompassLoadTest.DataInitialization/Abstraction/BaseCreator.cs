using System;
using DocVelocity.Integration.Encompass.API;

namespace EncompassLoadTest.DataInitialization
{
    public abstract class BaseCreator<TData> : ICreator
    {
        protected readonly IEncompassClient Client;
        protected TData Data;
        protected string ParentId;

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

        public abstract Try<IResult> Create();
    }
}