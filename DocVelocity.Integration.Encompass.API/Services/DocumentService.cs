using System.Collections.Generic;
using Elli.Api.Base;
using Elli.Api.Loans.EFolder.Api;
using Elli.Api.Loans.EFolder.Model;

namespace DocVelocity.Integration.Encompass.API.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly DocumentsApi _client;

        public DocumentService(AccessToken accessToken)
        {
            _client = ApiClientProvider.GetApiClient<DocumentsApi>(accessToken);
        }

        public EFolderDocumentContract GetDocument(string documentId, string loanId)
        {
            return _client.GetDocument(documentId, loanId);
        }

        public List<EFolderDocumentContract> GetLoanDocuments(string loanId)
        {
            return _client.GetDocuments(loanId);
        }

        public string CreateDocument(string loanId, EFolderDocumentContract document)
        {
            var response = _client.CreateDocumentWithHttpInfo(loanId, "id", document);
            return response.Headers["Location"].Split('/')[5];
        }

        public List<EFolderEntityRefContract> GetAttachmentsRefs(string documentId, string loanId)
        {
            return _client.RetrieveDocAttachments(documentId, loanId);
        }

        public void AttachAttachments(string documentId, string loanId, List<EFolderEntityRefContract> attachmentRefs)
        {
            _client.UpdateDocAttachments(documentId, loanId, "add", attachmentRefs);
        }

        public void DetachAttachments(string documentId, string loanId, List<EFolderEntityRefContract> attachmentRefs)
        {
            _client.UpdateDocAttachments(documentId, loanId, "remove", attachmentRefs);
        }
    }
}