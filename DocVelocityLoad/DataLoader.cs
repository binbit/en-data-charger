using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocVelocity.Integration.Helpers.Logging;
using DocVelocity.Orchestration.SDK;
using DocVelocity.Orchestration.SDK.Certificates;
using DocVelocity.Orchestration.SDK.Entities;
using DocVelocity.Orchestration.SDK.Extensions;
using DocVelocity.Orchestration.SDK.Settings;
using Newtonsoft.Json;

namespace DocVelocityLoad
{
    public class DataLoader
    {
        private readonly string _resultPath;
        private readonly LoadConfiguration _loadConfiguration;
        private readonly byte[] _data;
        private readonly CloudConfiguration _cloudSettings;
        private readonly ILogger _logger;
        private readonly IEnumerable<string> _loanIds;
        private readonly ICloud _cloud;
        private readonly ConcurrentBag<LoadResult> _result;

        public DataLoader(ILoggerFactory loggerFactory, string loadConfigurationPath, string resultPath)
        {
            _resultPath = resultPath;
            _logger = loggerFactory.GetLogger(nameof(DataLoader));
            _cloudSettings = JsonConvert.DeserializeObject<CloudConfiguration>(File.ReadAllText("cloud.settings.json"));
            _loadConfiguration =
                JsonConvert.DeserializeObject<LoadConfiguration>(File.ReadAllText(loadConfigurationPath));
            _data = File.ReadAllBytes(_loadConfiguration.FilePath);
            _loanIds = File.ReadLines(_loadConfiguration.LoanIdFilePath);
            _cloud = CreateCloud();
            _result = new ConcurrentBag<LoadResult>();
            LoadData();
        }

        public void LoadData()
        {
            Parallel.ForEach(_loanIds, new ParallelOptions {MaxDegreeOfParallelism = 8}, loanId =>
            {
                try
                {
                    var folderId = _cloud.Metadata.SearchMetadata(metaKey: "LOSLOANID", metaValue: $"{{{loanId}}}")
                        .FirstOrDefault()
                        ?.ObjectRef;

                    if (!string.IsNullOrEmpty(folderId))
                    {
                        for (int i = 0; i < _loadConfiguration.MailitemPerFolder; i++)
                        {
                            var mailItemId = UploadMailitem(_cloud, folderId);
                            _result.Add(new LoadResult
                            {
                                LoanId = loanId,
                                FolderId = folderId,
                                MailItemId = mailItemId
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            });
        }

        public void SaveResult()
        {
            var builder = new StringBuilder("LoanId,FolderId,MailitemId\n");
            foreach (var loadResult in _result)
            {
                builder.AppendLine($"{loadResult.LoanId},{loadResult.FolderId},{loadResult.MailItemId}");
            }

            var path = Path.Combine(_resultPath, $"mailitem-result-{(DateTime.Now.ToString("s"))}");
            File.WriteAllText(path, builder.ToString());
        }

        private ICloud CreateCloud()
        {
            return new Cloud(new CloudSettings
            {
                DefaultTicketManager = _cloudSettings.TicketManagerSettings,
                CertificateResolver = new FileCertificateResolver(_logger)
                {
                    FileName = _cloudSettings.CertificateFileName,
                    Password = _cloudSettings.CertificatePassword
                },
                Connection = _cloudSettings.ConnectionSettings,
                Download = new DownloadSettings(),
                Http = new HttpSettings(),
                Logger = _logger,
                Retry = new RetrySettings()

            });
        }

        private string UploadMailitem(ICloud cloud, string folderId)
        {
            var mailitem = new DocumentMailitem
            {
                Attachments = new List<DocumentAttachment>
                {
                    new DocumentAttachment
                    {
                        File = new AttachmentFile
                        {
                            Data = new MemoryStream(_data),
                            Name = "Test",
                            Type = "Test"
                        },
                        TypeDescription = "Test",
                        TypeName = "Test",
                        Dictionary = "unknown",
                        DateClientReceived = DateTime.UtcNow,

                    }
                },
                Channel = "load test",
                AutoCreateDocuments = true,
                IndexingOption = _loadConfiguration.IndexingOption,
                Subject = "load test",
                ContextId = folderId
            };

            return cloud.Mailitems.Upload(mailitem).Id;
        }
    }
}