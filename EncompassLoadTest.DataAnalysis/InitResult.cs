using System;

namespace EncompassLoadTest.DataAnalysis
{
    public class InitResult
    {
        public string LoanId { get; set; }
        public DateTime LoanCreateDateUtc { get; set; }
        public string DocumentId { get; set; }
        public DateTime? DocumentCreateDateUtc { get; set; }
        public string AttachmentId { get; set; }
        public DateTime? AttachmentCreateDateUtc { get; set; }
    }
}