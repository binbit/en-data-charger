namespace EncompassLoadTest.DataInitialization.Results
{
    public class AttachmentResult : BaseResult
    {
        public string AttachmentId => EntityId;
        public AttachmentResult(string attachmentId, string parentId) : base(attachmentId, parentId)
        {
        }
    }
}