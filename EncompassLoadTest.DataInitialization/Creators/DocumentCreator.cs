using DocVelocity.Integration.Encompass.API;
using Elli.Api.Loans.EFolder.Model;

namespace EncompassLoadTest.DataInitialization.Creators
{
    public class DocumentCreator : BaseCreator<EFolderDocumentContract, DocumentBaseResult>
    {
        public string LoanId => ParentId;

        public DocumentCreator(IEncompassClient client) : base(client)
        {
        }

        public override Try<DocumentBaseResult> Create()
        {
            return () =>
            {
                var documentId = Client.DocumentService.CreateDocument(ParentId, Data);
                return new DocumentBaseResult(documentId);
            };
        }
    }
}