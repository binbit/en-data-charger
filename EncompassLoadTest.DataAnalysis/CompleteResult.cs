using System;

namespace EncompassLoadTest.DataAnalysis
{
    public class CompleteResult : InitResult
    {
        public string FolderId { get; set; }
        public DateTime? FolderCreateDateUtc { get; set; }
        public string MailItemId { get; set; }
        public DateTime? MailItemCreateDateUtc { get; set; }
        public string DvDocumentId { get; set; }
        public DateTime? DvDocumentCreateDateUtc { get; set; }
        public AttachmentState AttachmentState { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public string NewDocumentId { get; set; }

        public CompleteResult()
        {
            
        }

        public CompleteResult(InitResult initResult)
        {
            LoanId = initResult.LoanId;
            LoanCreateDateUtc = initResult.LoanCreateDateUtc;
            DocumentId = initResult.DocumentId;
            DocumentCreateDateUtc = initResult.DocumentCreateDateUtc;
            AttachmentId = initResult.AttachmentId;
            AttachmentCreateDateUtc = initResult.AttachmentCreateDateUtc;
        }
    }
}