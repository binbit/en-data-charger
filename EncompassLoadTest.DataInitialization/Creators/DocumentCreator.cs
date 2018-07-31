using System;
using DocVelocity.Integration.Encompass.API;
using Elli.Api.Loans.EFolder.Model;
using EncompassLoadTest.DataInitialization.Results;
using Monad;
using NLog;

namespace EncompassLoadTest.DataInitialization.Creators
{
    public class DocumentCreator : BaseCreator<EFolderDocumentContract>
    {
        private readonly ILogger _logger;

        public string LoanId => ParentId;

        public DocumentCreator(IEncompassClient client, EFolderDocumentContract data, string loanId) 
            : base(client, data, loanId)
        {
            _logger = LogManager.GetLogger(nameof(DocumentCreator));
        }

        public override Try<IResult> Create(string parentId)
        {
            VerifyData();

            return () =>
            {
                try
                {
                    var documentId = Client.DocumentService.CreateDocument(ParentId, Data);
                    _logger.Info($"Document {documentId} created in loan {parentId}.");
                    return new DocumentResult(documentId, parentId);
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Error creating document in loan {parentId}.");
                    throw;
                }
            };
        }
    }
}