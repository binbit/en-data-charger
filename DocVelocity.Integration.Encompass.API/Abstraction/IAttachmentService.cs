using System.Collections.Generic;
using Elli.Api.Loans.EFolder.Model;

namespace DocVelocity.Integration.Encompass.API
{
    public interface IAttachmentService
    {
        EFolderAttachmentContract GetAttachment(string attachmentId, string loanId);
        List<EFolderAttachmentContract> GetAttachments(string loanId);
        string UploadAttachment(string loanId, string title, string extension, byte[] content);
    }
}