using System.Collections.Generic;
using System.Linq;

namespace EncompassLoadTest.DataInitialization.Results
{
    public class DocumentResult : BaseResult<AttachmentResult>
    {
        public string DocumentId => EntityId;

        public DocumentResult(string documentId, string parentId) : base(documentId, parentId)
        {
        }

        public override IEnumerable<AttachmentResult> GetInneResults()
        {
            return ResultCollection.Cast<AttachmentResult>();
        }
    }
}