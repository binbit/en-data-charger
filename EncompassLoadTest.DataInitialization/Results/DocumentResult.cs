namespace EncompassLoadTest.DataInitialization.Results
{
    public class DocumentResult : BaseResult
    {
        public string DocumentId => EntityId;

        public DocumentResult(string documentId, string parentId) : base(documentId, parentId)
        {
        }
    }
}