using System;

namespace EncompassLoadTest.DataAnalysis
{
    public class LoanStatisticResult
    {
        public string LoanId { get; set; }
        public DateTime? LoanCreateDateUtc { get; set; }
        public string FolderId { get; set; }
        public DateTime? FolderCreateDateUtc { get; set; }
        public int UploadedAttachments { get; set; }
        public int AttachmentInUploadContainer { get; set; }
        public int AttachmentsInCommonContainer { get; set; }
        public DateTime? AttachmentLastCreateDateUtc { get; set; }
    }
}