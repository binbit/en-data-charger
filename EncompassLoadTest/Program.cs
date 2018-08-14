using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using DocVelocity.Integration.Helpers.Logging;
using EncompassLoadTest.DataAnalysis;
using EncompassLoadTest.DataInitialization;
using EncompassLoadTest.DataInitialization.Results;
using NDesk.Options;

namespace EncompassLoadTest
{
    class Program
    {
        public enum Mode
        {
            Load,
            Analyse
        }

        private static ILogger _logger;
        private static string _loadResultFilePath;
        private static Mode _mode;
        private static string _analysResultFilePath;
        private static NLogLoggerFactory _loggerFactory;

        private static bool PrepareOptions(string[] args)
        {
            if (args.Length == 0)
                throw new Exception("No arguments passed for execution. Call --help to see more.");

            var showHelp = false;
            var p = new OptionSet()
            {
                {
                    "m|mode=", "Mode of tool. Use 'load' to load data to EN. Use 'analyse' to analyse results.",
                    v =>
                    {
                        _mode = (Mode)Enum.Parse(typeof(Mode), v, true);
                        _logger.Info($"Mode: {_mode}");
                    }
                },
                {
                    "lr|loadres=", "Path to load result file.",
                    v =>
                    {
                        _loadResultFilePath = v;
                        _logger.Info($"Path to load result file: {_loadResultFilePath}");
                    }
                },
                {
                    "ar|analysres=", "Path to analyse result file.",
                    v =>
                    {
                        _analysResultFilePath = v;
                        _logger.Info($"Path to analyse result file: {_analysResultFilePath}");
                    }
                },
                {
                    "h|help", "show this message and exit",
                    v =>
                    {
                        showHelp = v != null;
                    }
                }
            };

            var result = p.Parse(args);


            if (!showHelp) return false;
            ShowHelp(p);
            return true;
        }

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        static void Main(string[] args)
        {
            _loggerFactory = new NLogLoggerFactory();
            _logger = _loggerFactory.GetLogger("LoadTest");
            PrepareOptions(args);
            var config = ConfigurationManager.GetSection("ElliApiConfig");
            switch (_mode)
            {
                case Mode.Load:
                    InitData(config);
                    break;
                case Mode.Analyse:
                    AnalysData(config);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void AnalysData(object config)
        {
            var dataAnalyser = new DataAnalyser(_loggerFactory, config, _loadResultFilePath, _analysResultFilePath,
                "analysis.configuration.json", "cloud.settings.json");
            dataAnalyser.AnalysResults();
            dataAnalyser.WriteResult();
        }

        private static void InitData(object config)
        {
            var initializer = new DataInitializer(config, "load.configuration.json", _loggerFactory);
            var results = initializer.InitializeData().Cast<InitializationResult>();
            var strBuilder = new StringBuilder(
                "LoanId|LoanCreateDateUtc|DocumentId|DocumentCreateDateUtc|AttachmentId|AttachmentCreateDateUtc\n");

            foreach (var initializationResult in results)
            {
                foreach (var res in initializationResult.GetStringResult())
                {
                    strBuilder.AppendLine(res);
                }
            }

            var resultPath = Path.Combine(_loadResultFilePath, $"result-{DateTime.Now.ToString("s").Replace(":", "-")}.csv");
            File.WriteAllText(resultPath, strBuilder.ToString());
        }
    }
}
