using DocVelocity.Integration.Encompass.API;
using Elli.Api.Loans.EFolder.Model;
using EncompassLoadTest.DataInitialization.Results;
using Monad;

namespace EncompassLoadTest.DataInitialization.Creators
{
    public class DocumentCreator : BaseCreator<EFolderDocumentContract>
    {
        public string LoanId => ParentId;

        public DocumentCreator(IEncompassClient client, EFolderDocumentContract data, string loanId) 
            : base(client, data, loanId)
        {
        }

        public override Try<IResult> Create(string parentId)
        {
            VerifyData();

            return () =>
            {
                var documentId = Client.DocumentService.CreateDocument(ParentId, Data);
                return new DocumentResult(documentId, parentId);
            };
        }
    }
}