using System.Collections.Generic;
using Elli.Api.Loans.EFolder.Model;

namespace DocVelocity.Integration.Encompass.API
{
    public interface IDocumentService
    {
        EFolderDocumentContract GetDocument(string documentId, string loanId);
        List<EFolderDocumentContract> GetLoanDocuments(string loanId);
        string CreateDocument(string loanId, EFolderDocumentContract document);
        List<EFolderEntityRefContract> GetAttachmentsRefs(string documentId, string loanId);
        void AttachAttachments(string documentId, string loanId, List<EFolderEntityRefContract> attachmentRefs);
        void DetachAttachments(string documentId, string loanId, List<EFolderEntityRefContract> attachmentRefs);
    }
}