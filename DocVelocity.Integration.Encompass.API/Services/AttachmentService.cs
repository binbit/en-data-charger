using System.Collections.Generic;
using System.Linq;
using Elli.Api.Base;
using Elli.Api.Loans.EFolder.Api;
using Elli.Api.Loans.EFolder.Model;
using RestSharp;

namespace DocVelocity.Integration.Encompass.API.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly AttachmentsApi _client;

        public AttachmentService(AccessToken accessToken)
        {
            _client = ApiClientProvider.GetApiClient<AttachmentsApi>(accessToken);
        }

        public EFolderAttachmentContract GetAttachment(string attachmentId, string loanId)
        {
            return _client.GetAttachment(attachmentId, loanId);
        }

        public List<EFolderAttachmentContract> GetAttachments(string loanId)
        {
            return _client.GetAttachments(loanId);
        }

        public string UploadAttachment(string loanId, string title, string fileWithExtension, byte[] content)
        {
            return UploadAttachment(loanId, null, title, fileWithExtension, content);
        }

        public string UploadAttachment(string loanId, string documentId, string title, string fileWithExtension, byte[] content)
        {
            var attachmentInfo = _client.UploadAttachment(loanId, "id", new EFolderMediaUrlContract
            {
                CreateReason = 1,
                Title = title,
                FileWithExtension = fileWithExtension,
                DocumentRefId = documentId
            });

            var client = new RestClient(attachmentInfo.MediaUrl);
            var attachRequest = new RestRequest(Method.PUT);
            attachRequest.AddHeader("cache-control", "no-cache");
            attachRequest.AddParameter("undefined", content, ParameterType.RequestBody);
            var putResponse = client.Execute(attachRequest);
            return putResponse.Headers.First(h => h.Name == "Location").Value.ToString().Split('/')[5];
        }
    }
}