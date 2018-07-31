using System.Configuration;
using System.Linq;
using EncompassLoadTest.DataInitialization;
using EncompassLoadTest.DataInitialization.Results;

namespace EncompassLoadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationManager.GetSection("ElliApiConfig");
            var initializer = new DataInitializer(config, "load.configuration.json");
            var results = initializer.InitializeData().Cast<InitializationResult>();
        }
    }
}
