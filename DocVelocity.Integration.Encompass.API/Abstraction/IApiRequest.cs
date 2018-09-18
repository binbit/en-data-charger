using Elli.Api.Base;

namespace DocVelocity.Integration.Encompass.API
{
    internal interface IApiRequest<out TResponse> : ISecuredRequest
    {
        TResponse Invoke();
    }

    internal interface ISecuredRequest
    {
        AccessToken AccessToken { set; }
    }
}