using System.IO;
using System.Threading.Tasks;
using DocVelocity.Integration.Encompass.API;
using EncompassLoadTest.DataInitialization.Creators;
using Monad;

namespace EncompassLoadTest.DataInitialization.CreationBlocks
{
    public sealed class AttachmentCreationBlock : ICreationBlock
    {
        private readonly IEncompassClient _client;
        private readonly LoadConfiguration _loadConfiguration;
        private readonly byte[] _content;

        public AttachmentCreationBlock(IEncompassClient client, LoadConfiguration loadConfiguration)
        {
            _client = client;
            _loadConfiguration = loadConfiguration;
            _content = File.ReadAllBytes(loadConfiguration.AttachmentFilePath);
        }

        public async Task<IResult> CreateAsync(IResult result, string parentId)
        {
            var creator = new AttachmentCreator(_client, GetData(result.EntityId), parentId);

            for (var i = 0; i < _loadConfiguration.AttachmentCountPerDocument; i++)
            {
                //delay before creating attachment because API has some trouble with creating attachment immediately after document creation
                await Task.Delay(_loadConfiguration.AttachmentCreationDelay);
                var res = creator.Create(parentId);
                res.Match(Success: result.AddResult,
                    Fail: f => result.AddError(new ResultError(parentId, f))).Invoke();
            }

            return result;
        }

        private AttachmentData GetData(string documentId)
        {
            return new AttachmentData
            {
                Title = _loadConfiguration.AttachmentTitle,
                Content = _content,
                FileNameWithExtension = Path.GetFileName(_loadConfiguration.AttachmentFilePath),
                DocumentId = documentId
            };
        }
    }
}