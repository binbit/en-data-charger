using Elli.Api.Base;

namespace DocVelocity.Integration.Encompass.API
{
    internal interface ITokenProvider
    {
        AccessToken GetAccessToken();
    }
}