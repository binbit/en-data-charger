using DocVelocity.Orchestration.SDK.Entities.Enums;

namespace DocVelocityLoad
{
    public class LoadConfiguration
    {
        public string FilePath { get; set; }
        public int MailitemPerFolder { get; set; }
        public IndexingOption IndexingOption { get; set; }
        public string LoanIdFilePath { get; set; }
    }
}