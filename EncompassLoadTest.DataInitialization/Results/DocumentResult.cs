using System;
using System.Collections.Generic;
using EncompassLoadTest.DataInitialization.Errors;
using EncompassLoadTest.DataInitialization.Results;

namespace EncompassLoadTest.DataInitialization
{
    public class DocumentResult : Result<AttachmentResult, AttachmentError>
    {
        public string DocumentId => EntityId;
        public IReadOnlyCollection<AttachmentResult> AttachmentResults => ResultCollection;
        public IReadOnlyCollection<AttachmentError> AttachmentErrors => ErrorCollection;

        public DocumentResult(string documentId) : base(documentId)
        {
        }
    }
}