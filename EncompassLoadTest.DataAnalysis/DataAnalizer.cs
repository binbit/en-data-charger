using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class DataAnalizer
    {
        public DataAnalizer(object configuration, string initResultPath, string analysisConfig, string cloudSettingsConfig)
        {
            var loggerFactory = new NLogLoggerFactory();
            var logger = loggerFactory.GetLogger(nameof(DataAnalizer));
            var analysisConfiguration =
                JsonConvert.DeserializeObject<AnalysisConfiguration>(File.ReadAllText(analysisConfig));
            var cloudSettings =
                JsonConvert.DeserializeObject<CloudConfiguration>(File.ReadAllText(cloudSettingsConfig));

            var cloud = new Cloud(new CloudSettings
            {
                DefaultTicketManager = cloudSettings.TicketManagerSettings,
                CertificateResolver = new FileCertificateResolver(logger),
                Connection = cloudSettings.ConnectionSettings,
                Download = cloudSettings.DownloadSettings,
                Http = new HttpSettings
                {
                    WebRequest = cloudSettings.HttpWebRequestSettings
                },
                Logger = logger,
                Retry = cloudSettings.RetrySettings
            });

            var initResults = CsvParser.GetRecordsFromCsv<InitResult>(initResultPath, true);
            var completeResults = new List<CompleteResult>();
            var client = new EncompassClient(configuration);
            foreach (var loanGroup in initResults.GroupBy(r=>r.LoanId))
            {
                try
                {
                    var loanId = loanGroup.Key;
                    var folderId = GetFolderId(cloud, loanId);
                    if (!string.IsNullOrEmpty(folderId))
                    {
                        var folder = cloud.Folders.Get(folderId);
                        if (folder != null)
                        {
                            var attachments = client.AttachmentService.GetAttachments(loanId);
                            foreach (var attachment in attachments)
                            {
                                var document =
                                    client.DocumentService.GetDocument(attachment.Document.EntityName, loanId);

                                var mailItemId = GetMailItemId(cloud, attachment);

                                ReceivedMailItem mailItem = null;
                                if (!string.IsNullOrEmpty(mailItemId))
                                    mailItem = cloud.Mailitems.GetReceivedMailItem(mailItemId);

                                var dvDocumentId = GetDvDocumentId(cloud, attachment);

                                Document dvDocument = null;
                                if (!string.IsNullOrEmpty(dvDocumentId))
                                {
                                    dvDocument = cloud.Documents.Get(dvDocumentId);
                                }

                                var attachmentState = GetAttachmentState(analysisConfiguration, document);

                                var originalAttachmentResult =
                                    loanGroup.FirstOrDefault(l => l.AttachmentId == attachment.AttachmentId);
                                CompleteResult result = null;
                                if (originalAttachmentResult != null)
                                {
                                    result = new CompleteResult(originalAttachmentResult)
                                    {
                                        AttachmentState = attachmentState,
                                        AttachmentType = AttachmentType.Original,
                                        NewDocumentId = document.DocumentId,
                                        MailItemId = mailItem?.Id,
                                        MailItemCreateDateUtc = mailItem?.DateCreated.ToUniversalTime(),
                                        FolderId = folder?.Id,
                                        FolderCreateDateUtc = folder?.DateCreated.ToUniversalTime(),
                                        DvDocumentId = dvDocument.Id,
                                        DvDocumentCreateDateUtc = dvDocument.DateCreated?.ToUniversalTime()
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
                                        DvDocumentId = dvDocument.Id,
                                        DvDocumentCreateDateUtc = dvDocument.DateCreated?.ToUniversalTime(),
                                        AttachmentId = attachment.AttachmentId,
                                        AttachmentCreateDateUtc = attachment.DateCreated?.ToUniversalTime()
                                    };
                                }

                                completeResults.Add(result);
                            }
                        }
                        else
                        {
                            completeResults.AddRange(loanGroup.Select(lg => new CompleteResult(lg)));
                        }
                    }
                    else
                    {
                        completeResults.AddRange(loanGroup.Select(lg => new CompleteResult(lg)));
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            }

            var path = $"D:\\analysis-result-{DateTime.Now.ToString("s").Replace(":", "-")}.csv";
            CsvParser.WriteToCsv(completeResults, path);
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

        private static string GetDvDocumentId(Cloud cloud, Elli.Api.Loans.EFolder.Model.EFolderAttachmentContract attachment)
        {
            return cloud.Metadata
                                                .SearchMetadata(metaKey: "LOSATTACHMENTID", metaValue: attachment.AttachmentId)
                                                .FirstOrDefault(m => m.ObjectType == ObjectType.DOCUMENT)?.ObjectRef;
        }

        private static string GetMailItemId(Cloud cloud, Elli.Api.Loans.EFolder.Model.EFolderAttachmentContract attachment)
        {
            return cloud.Metadata
                                                .SearchMetadata(metaKey: "LOSATTACHMENTID", metaValue: attachment.AttachmentId)
                                                .FirstOrDefault(m => m.ObjectType == ObjectType.MAILITEM)?.ObjectRef;
        }

        private static string GetFolderId(Cloud cloud, string loanId)
        {
            return cloud.Metadata.SearchMetadata(metaKey: "LOSLOANID", metaValue: loanId)
                                    .FirstOrDefault(m => m.ObjectType == ObjectType.FOLDER)?.ObjectRef;
        }
    }
}