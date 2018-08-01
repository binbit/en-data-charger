using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
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
            var strBuilder = new StringBuilder(
                "Instance Id|Create Date UTC|Loan ID|Create Date UTC|"
                + "Document Id|Create Date UTC|Attachment Id|Create Date UTC\n");

            foreach (var initializationResult in results)
            {
                foreach (var res in initializationResult.GetStringResult())
                {
                    strBuilder.AppendLine(res);
                }
            }

            var resultPath = $"D:/result-{DateTime.Now.ToString("s").Replace(":", "-")}.csv";
            File.WriteAllText(resultPath, strBuilder.ToString());
            File.ReadAllLines(resultPath);
            var t = new DataTable();
            
        }
    }
}
