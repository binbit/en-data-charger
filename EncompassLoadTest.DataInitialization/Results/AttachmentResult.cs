using EncompassLoadTest.DataInitialization.Errors;

namespace EncompassLoadTest.DataInitialization.Results
{
    public class AttachmentResult : BaseResult
    {
        public string AttachmentId => EntityId;
        public AttachmentResult(string attachmentId) : base(attachmentId)
        {
        }
    }
}