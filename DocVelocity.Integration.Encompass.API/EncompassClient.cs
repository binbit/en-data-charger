using System.Configuration;
using System.Net;
using DocVelocity.Integration.Encompass.API.Services;
using Elli.Api.Base;

namespace DocVelocity.Integration.Encompass.API
{
    public class EncompassClient : IEncompassClient
    {
        public ILoanService LoanService { get; }
        public IDocumentService DocumentService { get; }
        public IAttachmentService AttachmentService { get; }
        public ILockService LockService { get; }
        public IPipelineService PipelineService { get; }

        public EncompassClient(object config)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var configuration = (ApiConfiguration) config;
            var credentials = new UserCredential
            {
                InstanceId = configuration.InstanceId,
                IdentityType = IdentityType.Lender,
                Password = configuration.Password,
                UserName = configuration.Username
            };

            //todo:create token manager to keep token alive or check it status and revoke
            var token = AccessToken.GetAccessToken(credentials, configuration.ApiClientId, configuration.ClientSecret);

            LoanService = new LoanService(token);
            DocumentService = new DocumentService(token);
            AttachmentService = new AttachmentService(token);
        }
    }
}