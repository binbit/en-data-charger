using DocVelocity.Integration.Encompass.API;
using EncompassLoadTest.DataInitialization.Results;
using Monad;

namespace EncompassLoadTest.DataInitialization.Creators
{
    public class AttachmentCreator : BaseCreator<AttachmentData>
    {
        public AttachmentCreator(IEncompassClient client, AttachmentData data, string loanId) 
            : base(client, data, loanId)
        {
        }

        public override Try<IResult> Create(string parentId)
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

                return new AttachmentResult(attachmentId, parentId);
            };
        }
    }
}