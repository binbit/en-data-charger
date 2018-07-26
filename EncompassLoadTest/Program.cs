using System.Configuration;
using DocVelocity.Integration.Encompass.API;

namespace EncompassLoadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationManager.GetSection("ElliApiConfig");
            var client = new EncompassClient(config);
        }
    }
}
