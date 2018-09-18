using System;

namespace DocVelocity.Integration.Encompass.API.Caller
{
    internal class RetryApiCaller : IApiCaller
    {
        public (TResponse, Exception) CallApi<TRequest, TResponse>(TRequest request) where TRequest : IApiRequest<TResponse>
        {
            throw new NotImplementedException();
        }
    }
}