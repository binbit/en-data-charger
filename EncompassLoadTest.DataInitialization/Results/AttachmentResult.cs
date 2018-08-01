using System.Collections.Generic;

namespace EncompassLoadTest.DataInitialization.Results
{
    public class AttachmentResult : BaseResult<NoResult>
    {
        public string AttachmentId => EntityId;
        public AttachmentResult(string attachmentId, string parentId) : base(attachmentId, parentId)
        {
        }
    }
}