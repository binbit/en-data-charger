using System;
using DocVelocity.Integration.Encompass.API;
using EncompassLoadTest.DataInitialization.Results;
using Monad;
using NLog;

namespace EncompassLoadTest.DataInitialization.Creators
{
    public class AttachmentCreator : BaseCreator<AttachmentData>
    {
        private readonly ILogger _logger;

        public AttachmentCreator(IEncompassClient client, AttachmentData data, string loanId) 
            : base(client, data, loanId)
        {
            _logger = LogManager.GetLogger(nameof(AttachmentCreator));
        }

        public override Try<IResult> Create(string parentId)
        {
            VerifyData();

            return () =>
            {
                try
                {
                    var attachmentId = Client.AttachmentService.UploadAttachment(
                        ParentId,
                        Data.DocumentId,
                        Data.Title,
                        Data.FileNameWithExtension,
                        Data.Content);
                    _logger.Info($"Attachment {attachmentId} created for document {Data.DocumentId} in loan {parentId}.");
                    return new AttachmentResult(attachmentId, parentId);
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Error creating attachment for document {Data.DocumentId} in loan {parentId}.");
                    throw;
                }
            };
        }
    }
}