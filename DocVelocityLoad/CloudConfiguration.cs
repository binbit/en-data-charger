using DocVelocity.Orchestration.SDK.Settings;

namespace DocVelocityLoad
{
    public class CloudConfiguration
    {
        public ConnectionSettings ConnectionSettings { get; set; }
        public TicketManagerSettings TicketManagerSettings { get; set; }
        public DownloadSettings DownloadSettings { get; set; }
        public RetrySettings RetrySettings { get; set; }
        public HttpWebRequestSettings HttpWebRequestSettings { get; set; }
        public string CertificateFileName { get; set; }
        public string CertificatePassword { get; set; }
    }
}