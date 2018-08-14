using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;

namespace EncompassLoadTest.DataAnalysis
{
    public class CsvParser
    {
        public static IEnumerable<T> GetRecordsFromCsv<T>(string filePath, bool hasHeader)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.Delimiter = "|";
                csv.Configuration.HasHeaderRecord = hasHeader;
                return csv.GetRecords<T>().ToList();
            }
        }

        public static void WriteToCsv<T>(IEnumerable<T> recaords, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteHeader<T>();
                csv.WriteRecords(recaords);
            }
        }
    }
}