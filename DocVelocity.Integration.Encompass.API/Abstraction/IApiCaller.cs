using System;

namespace DocVelocity.Integration.Encompass.API
{
    internal interface IApiCaller
    {
        (TResponse, Exception) CallApi<TRequest, TResponse>(TRequest request)
            where TRequest : IApiRequest<TResponse>;
    }
}