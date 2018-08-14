using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocVelocity.Integration.Encompass.API;
using Elli.Api.Loans.EFolder.Model;
using EncompassLoadTest.DataInitialization.Creators;
using Monad;

namespace EncompassLoadTest.DataInitialization.CreationBlocks
{
    public class DocumentCreationBlock : ICreationBlock
    {
        private readonly IEncompassClient _client;
        private readonly LoadConfiguration _loadConfiguration;
        private readonly AttachmentCreationBlock _attachmentCreationBlock;

        public DocumentCreationBlock(IEncompassClient client, LoadConfiguration loadConfiguration)
        {
            _client = client;
            _loadConfiguration = loadConfiguration;
            _attachmentCreationBlock = new AttachmentCreationBlock(client, loadConfiguration);
        }

        public async Task<IResult> CreateAsync(IResult result, string parentId)
        {
            var creator = new DocumentCreator(_client, GetData(), parentId);
            var attTasks = new List<Task<IResult>>();
            for (var i = 0; i < _loadConfiguration.DocumentCountPerLoan; i++)
            {
                var res = creator.Create(parentId);
                res.Match(Success:async r =>
                    {
                        result.AddResult(r);
                        attTasks.Add(Task.Run(() => _attachmentCreationBlock.CreateAsync(r, parentId)));
                        await Task.Delay(_loadConfiguration.DocumentCreationDelay);
                    },
                    Fail: f => result.AddError(new ResultError(parentId, f))).Invoke();

            }

            await Task.WhenAll(attTasks);

            return result;
        }

        private EFolderDocumentContract GetData()
        {
            var now = DateTime.Now;
            return new EFolderDocumentContract
            {
                Title = _loadConfiguration.DocumentTitle,
                Description = "It test document 8-)",
                ApplicationId = "All",
                RequestedFrom = "User",
                EmnSignature = "string",
                DateRequested = now,
                DateExpected = now,
                DateReceived = now,
                DateReviewed = now,
                DateReadyForUw = now,
                DateReadyToShip = now,
                Comments = new List<EFolderDocumentContractComments>
                {
                    new EFolderDocumentContractComments
                    {
                        Comments = "Lalala"
                    }
                }
            };
        }
    }
}