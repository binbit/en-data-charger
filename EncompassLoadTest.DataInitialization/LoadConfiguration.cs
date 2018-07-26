using System;

namespace EncompassLoadTest.DataInitialization
{
    public class LoadConfiguration
    {
        public int InstanceCount { get; set; }
        public int LoanNumberPerInstance { get; set; }
        public TimeSpan LoanCreationDelay { get; set; }
        public string DocumentTitle { get; set; }
        public int DocumentCountPerLoan { get; set; }
        public TimeSpan DocumentCreationDelay { get; set; }
        public string AttachmentTitle { get; set; }
        public int AttachmentCountPerDocument { get; set; }
        public TimeSpan AttachmentCreationDelay { get; set; }
        public string AttachmentFilePath { get; set; }
    }
}