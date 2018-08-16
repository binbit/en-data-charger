using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocVelocity.Integration.Encompass.API;
using DocVelocity.Integration.Helpers.Logging;
using DocVelocity.Orchestration.SDK;
using DocVelocity.Orchestration.SDK.Certificates;
using DocVelocity.Orchestration.SDK.Entities;
using DocVelocity.Orchestration.SDK.Extensions;
using DocVelocity.Orchestration.SDK.Settings;
using Newtonsoft.Json;

namespace EncompassLoadTest.DataAnalysis
{
    public class DataAnalyser
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly object _configuration;
        private readonly string _initResultPath;
        private readonly string _analysResultPath;
        private readonly Cloud _cloud;
        private readonly AnalysisConfiguration _analysisConfiguration;
        private readonly ILogger _logger;
        private readonly ConcurrentBag<CompleteResult> _completeResults;
        private readonly ConcurrentBag<LoanStatisticResult> _loanResults;

        public DataAnalyser(ILoggerFactory loggerFactory, object configuration, string initResultPath,
            string analysResultPath, string analysisConfig, string cloudSettingsConfig)
        {
            _completeResults = new ConcurrentBag<CompleteResult>();
            _loanResults = new ConcurrentBag<LoanStatisticResult>();
            _loggerFactory = loggerFactory;
            _configuration = configuration;
            _initResultPath = initResultPath;
            _analysResultPath = analysResultPath;
            _logger = loggerFactory.GetLogger(nameof(DataAnalyser));
            _analysisConfiguration =
                JsonConvert.DeserializeObject<AnalysisConfiguration>(File.ReadAllText(analysisConfig));
            var cloudSettings =
                JsonConvert.DeserializeObject<CloudConfiguration>(File.ReadAllText(cloudSettingsConfig));

            _cloud = new Cloud(new CloudSettings
            {
                DefaultTicketManager = cloudSettings.TicketManagerSettings,
                CertificateResolver = new FileCertificateResolver(_logger)
                {
                    FileName = cloudSettings.CertificateFileName,
                    Password = cloudSettings.CertificatePassword
                },
                Connection = cloudSettings.ConnectionSettings,
                Download = new DownloadSettings(),
                Http = new HttpSettings(),
                Logger = _logger,
                Retry = new RetrySettings()

            });
        }

        public void WriteResult()
        {
            var path = Path.Combine(_analysResultPath,
                            $"analysis-result-{DateTime.Now.ToString("s").Replace(":", "-")}.csv");
            CsvParser.WriteToCsv(_completeResults, path);
            path = Path.Combine(_analysResultPath,
                $"loan-result-{DateTime.Now.ToString("s").Replace(":", "-")}.csv");
            CsvParser.WriteToCsv(_loanResults, path);
        }

        public void AnalysResults()
        {
            var initResults = CsvParser.GetRecordsFromCsv<InitResult>(_initResultPath, true);
            var loanGroups = initResults.GroupBy(r => r.LoanId);
            Parallel.ForEach(loanGroups, new ParallelOptions{MaxDegreeOfParallelism = 8}, loanGroup =>
            {
                var client = new EncompassClient(_configuration, _loggerFactory);

                try
                {
                    var loanResult = new LoanStatisticResult
                    {
                        UploadedAttachments = loanGroup.Count(),
                        LoanCreateDateUtc = loanGroup.First().LoanCreateDateUtc
                    };
                    var loanId = loanResult.LoanId = loanGroup.Key;
                    var folderId = GetFolderId(_cloud, loanId);
                    if (!string.IsNullOrEmpty(folderId))
                    {
                        var folder = _cloud.Folders.Get(folderId);
                        if (folder != null)
                        {
                            loanResult.FolderId = folderId;
                            loanResult.FolderCreateDateUtc = folder.DateCreated.ToUniversalTime();
                            var documents = client.DocumentService.GetLoanDocuments(loanId);
                            DateTime? lastAttachmentDate = null;
                            foreach (var document in documents)
                            {
                                var attachmentRefs =
                                    client.DocumentService.GetAttachmentsRefs(document.DocumentId, loanId);
                                foreach (var attachmentRef in attachmentRefs)
                                {
                                    var attachment =
                                        client.AttachmentService.GetAttachment(attachmentRef.EntityId, loanId);

                                    if (lastAttachmentDate == null || lastAttachmentDate < attachment.DateCreated)
                                        lastAttachmentDate = attachment.DateCreated;

                                    var mailItemId = GetMailItemId(_cloud, attachment);

                                    ReceivedMailItem mailItem = null;
                                    if (!string.IsNullOrEmpty(mailItemId))
                                        mailItem = _cloud.Mailitems.GetReceivedMailItem(mailItemId);

                                    var dvDocumentId = GetDvDocumentId(_cloud, attachment);

                                    Document dvDocument = null;
                                    if (!string.IsNullOrEmpty(dvDocumentId))
                                    {
                                        dvDocument = _cloud.Documents.Get(dvDocumentId);
                                    }

                                    var attachmentState = GetAttachmentState(_analysisConfiguration, document);

                                    switch (attachmentState)
                                    {
                                        case AttachmentState.UploadContainer:
                                            loanResult.AttachmentInUploadContainer++;
                                            break;
                                        case AttachmentState.CommonContainer:
                                            loanResult.AttachmentsInCommonContainer++;
                                            break;
                                    }

                                    var originalAttachmentResult =
                                        loanGroup.FirstOrDefault(l => l.AttachmentId == attachment.AttachmentId);
                                    CompleteResult result = null;
                                    if (originalAttachmentResult != null)
                                    {
                                        result = new CompleteResult(originalAttachmentResult)
                                        {
                                            AttachmentState = attachmentState,
                                            AttachmentType = AttachmentType.Original,
                                            NewDocumentId = document?.DocumentId,
                                            MailItemId = mailItem?.Id,
                                            MailItemCreateDateUtc = mailItem?.DateCreated.ToUniversalTime(),
                                            FolderId = folder?.Id,
                                            FolderCreateDateUtc = folder?.DateCreated.ToUniversalTime(),
                                            DvDocumentId = dvDocument?.Id,
                                            DvDocumentCreateDateUtc = dvDocument?.DateCreated?.ToUniversalTime()
                                        };
                                    }
                                    else
                                    {
                                        result = new CompleteResult
                                        {
                                            LoanId = loanId,
                                            LoanCreateDateUtc = loanGroup.First().LoanCreateDateUtc,
                                            AttachmentState = attachmentState,
                                            AttachmentType = AttachmentType.FromDocVelocity,
                                            DocumentId = document?.DocumentId,
                                            DocumentCreateDateUtc = document?.DateCreated?.ToUniversalTime(),
                                            MailItemId = mailItem?.Id,
                                            MailItemCreateDateUtc = mailItem?.DateCreated.ToUniversalTime(),
                                            FolderId = folder?.Id,
                                            FolderCreateDateUtc = folder?.DateCreated.ToUniversalTime(),
                                            DvDocumentId = dvDocument?.Id,
                                            DvDocumentCreateDateUtc = dvDocument?.DateCreated?.ToUniversalTime(),
                                            AttachmentId = attachment.AttachmentId,
                                            AttachmentCreateDateUtc = attachment.DateCreated?.ToUniversalTime()
                                        };
                                    }

                                    _completeResults.Add(result);
                                }
                            }

                            loanResult.AttachmentLastCreateDateUtc = lastAttachmentDate?.ToUniversalTime();
                        }
                        else
                        {
                            loanGroup.Select(lg => new CompleteResult(lg)).ToList()
                                .ForEach(l => _completeResults.Add(l));
                        }
                    }
                    else
                    {
                        loanGroup.Select(lg => new CompleteResult(lg)).ToList()
                            .ForEach(l => _completeResults.Add(l));
                    }

                    _loanResults.Add(loanResult);
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            });
        }

        private static AttachmentState GetAttachmentState(AnalysisConfiguration analysisConfiguration,
            Elli.Api.Loans.EFolder.Model.EFolderDocumentContract document)
        {
            var attachmentState = AttachmentState.Unassigned;
            if (document != null)
            {
                if (document.Title.Equals(analysisConfiguration.UploadContainer,
                    StringComparison.OrdinalIgnoreCase))
                    attachmentState = AttachmentState.UploadContainer;
                if (document.Title.Equals(analysisConfiguration.DestinationContainer,
                    StringComparison.OrdinalIgnoreCase))
                    attachmentState = AttachmentState.CommonContainer;
            }

            return attachmentState;
        }

        private static string GetDvDocumentId(Cloud cloud,
            Elli.Api.Loans.EFolder.Model.EFolderAttachmentContract attachment)
        {
            return cloud.Metadata
                .SearchMetadata(metaKey: "LOSATTACHMENTID", metaValue: attachment.AttachmentId)
                .FirstOrDefault(m => m.ObjectType == ObjectType.DOCUMENT)?.ObjectRef;
        }

        private static string GetMailItemId(Cloud cloud,
            Elli.Api.Loans.EFolder.Model.EFolderAttachmentContract attachment)
        {
            return cloud.Metadata
                .SearchMetadata(metaKey: "LOSATTACHMENTID", metaValue: attachment.AttachmentId)
                .FirstOrDefault(m => m.ObjectType == ObjectType.MAILITEM)?.ObjectRef;
        }

        private static string GetFolderId(Cloud cloud, string loanId)
        {
            return cloud.Metadata.SearchMetadata(metaKey: "LOSLOANID", metaValue: $"{{{loanId}}}")
                .FirstOrDefault(m => m.ObjectType == ObjectType.FOLDER)?.ObjectRef;
        }
    }
}