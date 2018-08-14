using System;
using System.Collections.Generic;
using System.Linq;
using DocVelocity.Integration.Helpers.Logging;
using Elli.Api.Base;
using Elli.Api.Loans.EFolder.Api;
using Elli.Api.Loans.EFolder.Model;
using RestSharp;

namespace DocVelocity.Integration.Encompass.API.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly AttachmentsApi _client;
        private readonly ILogger _logger;

        public AttachmentService(AccessToken accessToken, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.GetLogger(nameof(AttachmentService));
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
            var attachmentInfo = _client.UploadAttachmentWithHttpInfo(loanId, "id", new EFolderMediaUrlContract
            {
                CreateReason = 1,
                Title = title,
                FileWithExtension = fileWithExtension,
                DocumentRefId = documentId
            });
            var client = new RestClient(attachmentInfo.Data.MediaUrl);
            var attachRequest = new RestRequest(Method.PUT);
            attachRequest.AddHeader("cache-control", "no-cache");
            attachRequest.AddParameter("undefined", content, ParameterType.RequestBody);
            IRestResponse putResponse = null;
            try
            {
                putResponse = client.Execute(attachRequest);
                return putResponse.Headers.First(h => h.Name == "Location").Value.ToString().Split('/')[5];
            }
            catch (Exception e)
            {
                _logger.Error(LogEntry.New($"Error uploading data for attachment.\n{ParseResponse(putResponse)}").WithException(e));
                throw;
            }
        }

        private string ParseResponse(IRestResponse putResponse)
        {
            return
                $"{putResponse.Content}\n{string.Join("\n", putResponse.Headers.Select(h => $"{h.Name}:{h.Value}"))}";
        }
    }
}