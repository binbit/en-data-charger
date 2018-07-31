using System.Configuration;
using EncompassLoadTest.DataInitialization;

namespace EncompassLoadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationManager.GetSection("ElliApiConfig");
            var initializer = new DataInitializer(config, "load.configuration.json");
            var results = initializer.InitializeData();
        }
    }
}
