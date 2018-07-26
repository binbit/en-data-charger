using EncompassLoadTest.DataInitialization.Errors;

namespace EncompassLoadTest.DataInitialization.Results
{
    public class AttachmentBaseResult : BaseResult<NoResult, NoError>
    {
        public string AttachmentId => EntityId;
        public AttachmentBaseResult(string attachmentId) : base(attachmentId)
        {
        }
    }
}