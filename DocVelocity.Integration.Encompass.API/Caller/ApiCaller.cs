using System;

namespace DocVelocity.Integration.Encompass.API.Caller
{
    internal class ApiCaller : IApiCaller
    {
        private readonly ITokenProvider _tokenProvider;

        public ApiCaller(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public (TResponse, Exception) CallApi<TRequest, TResponse>(TRequest request) where TRequest : IApiRequest<TResponse>
        {
            try
            {
                var token = _tokenProvider.GetAccessToken();
                request.AccessToken = token;
                return (request.Invoke(), null);
            }
            catch (Exception ex)
            {
                return (default(TResponse), ex);
            }
        }
    }
}