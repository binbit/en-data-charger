using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DocVelocity.Integration.Encompass.API;
using EncompassLoadTest.DataInitialization.CreationBlocks;
using EncompassLoadTest.DataInitialization.Results;
using Newtonsoft.Json;
using NLog;

namespace EncompassLoadTest.DataInitialization
{
    public class DataInitializer : IDataInitializer
    {
        private readonly object _configuration;
        private readonly LoadConfiguration _loadConfiguration;
        private readonly ILogger _logger;

        public DataInitializer(object configuration, string loadConfig)
        {
            _configuration = configuration;
            _loadConfiguration = JsonConvert.DeserializeObject<LoadConfiguration>(File.ReadAllText(loadConfig));
            _logger = LogManager.GetLogger(nameof(DataInitializer));
        }

        public List<IResult> InitializeData()
        {
            _logger.Info("Start data load");
            var tasks = new List<Task>();
            var results = new List<IResult>();
            for (int i = 0; i < _loadConfiguration.InstanceCount; i++)
            {
                var client = new EncompassClient(_configuration);
                var loanCreationBlock = new LoanCreationBlock(client, _loadConfiguration);
                var instanceId = Guid.NewGuid().ToString();
                var initResult = new InitializationResult(instanceId);
                results.Add(initResult);
                tasks.Add(Task.Run(() => loanCreationBlock.CreateAsync(initResult, instanceId)));
            }

            Task.WaitAll(tasks.ToArray());

            return results;
        }
    }
}