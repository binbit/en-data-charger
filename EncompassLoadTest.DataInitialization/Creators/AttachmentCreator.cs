using DocVelocity.Integration.Encompass.API;
using EncompassLoadTest.DataInitialization.Results;

namespace EncompassLoadTest.DataInitialization.Creators
{
    public class AttachmentCreator : BaseCreator<AttachmentData, AttachmentBaseResult>
    {
        public AttachmentCreator(IEncompassClient client) : base(client)
        {
        }

        public override Try<AttachmentBaseResult> Create()
        {
            VerifyData();

            return () =>
            {
                var attachmentId = Client.AttachmentService.UploadAttachment(
                    ParentId,
                    Data.DocumentId,
                    Data.Title,
                    Data.FileNameWithExtension,
                    Data.Content);

                return new AttachmentBaseResult(attachmentId);
            };
        }
    }
}