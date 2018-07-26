using System;

namespace EncompassLoadTest.DataInitialization.Errors
{
    public class AttachmentError : ResultError
    {
        public string DocumentId => ParentId;

        public AttachmentError(string documentId, Exception exception) : base(documentId, exception)
        {
        }
    }
}