using System;
using System.Collections.Generic;
using System.Linq;
using EncompassLoadTest.DataInitialization.Errors;
using EncompassLoadTest.DataInitialization.Results;

namespace EncompassLoadTest.DataInitialization
{
    public class DocumentResult : BaseResult
    {
        public string DocumentId => EntityId;

        public DocumentResult(string documentId) : base(documentId)
        {
        }
    }
}