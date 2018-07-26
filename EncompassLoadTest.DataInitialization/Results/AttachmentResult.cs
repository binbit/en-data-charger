using EncompassLoadTest.DataInitialization.Errors;

namespace EncompassLoadTest.DataInitialization.Results
{
    public class AttachmentResult : Result<NoResult, NoError>
    {
        public string AttachmentId => EntityId;
        public AttachmentResult(string attachmentId) : base(attachmentId)
        {
        }
    }
}