using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace CoffeeMorningAssigner.DAL
{
    public class CsvRepository<T> : Repository
    {
        public List<T> Load(string filename, bool checkFileExists = true)
        {
            var filePath = FilePath(filename);

            Console.WriteLine($"Loading data from {filePath}");

            EnsureFileExists(filePath, checkFileExists);

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<T>().ToList();
        }

        public void Save(string filename, IEnumerable<T> records)
        {
            var filePath = FilePath(filename);
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords<T>(records);
        }
    }
}