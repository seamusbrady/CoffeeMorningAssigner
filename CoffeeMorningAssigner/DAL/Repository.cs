using System;
using System.IO;

namespace CoffeeMorningAssigner.DAL
{
    public abstract class Repository
    {
        protected string FilePath(string filename)
        {
            var filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath = Path.Join(filePath, "CoffeeMornings");
            filePath = Path.Join(filePath, filename);
            return filePath;
        }

        protected static void EnsureFileExists(string filePath, bool checkFileExists)
        {
            if (checkFileExists == false) return;
            if (File.Exists(filePath)) return;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("The application is trying to load the data but it cannot find the file at:");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(filePath);
            Console.ResetColor();
            Environment.Exit(-1);
        }
    }
}