using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace CoffeeMorningAssigner.DAL
{
    public class CsvRepository<T>
    {
        /// <summary>
        /// The root folder (where all the working files are stored) is set to MyDocuments
        /// Files will be copied and generated into MyDocuments\CoffeeMornings
        /// </summary>
        private const Environment.SpecialFolder RootFolder = Environment.SpecialFolder.MyDocuments;

        public List<T> Load(string filename)
        {
            var filePath = FilePath(filename);

            Console.WriteLine($"Loading data from {filePath}");

            EnsureFileExists(filePath);

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

        protected static string FilePath(string filename)
        {
            var filePath = Environment.GetFolderPath(RootFolder);
            filePath = Path.Join(filePath, "CoffeeMornings");
            filePath = Path.Join(filePath, filename);
            return filePath;
        }

        protected static void EnsureFileExists(string filePath)
        {
            if (File.Exists(filePath)) return;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("The application is trying to load the data but it cannot find the file at:");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(filePath);
            Console.ResetColor();
            Environment.Exit(-1);
        }


        public static void CopyFileToWorkingFolder(string filename)
        {
            var filePath = FilePath(new FileInfo(filename).Name);
            if (File.Exists(filePath)) return;
            File.Copy(filename, filePath);
        }
    }
}