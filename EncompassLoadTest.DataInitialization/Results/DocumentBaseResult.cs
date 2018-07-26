using System;
using System.Collections.Generic;
using EncompassLoadTest.DataInitialization.Errors;
using EncompassLoadTest.DataInitialization.Results;

namespace EncompassLoadTest.DataInitialization
{
    public class DocumentBaseResult : BaseResult<AttachmentBaseResult, AttachmentError>
    {
        public string DocumentId => EntityId;
        public IReadOnlyCollection<AttachmentBaseResult> AttachmentResults => ResultCollection;
        public IReadOnlyCollection<AttachmentError> AttachmentErrors => ErrorCollection;

        public DocumentBaseResult(string documentId) : base(documentId)
        {
        }
    }
}