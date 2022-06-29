using System;
using System.IO;

namespace CoffeeMorningAssigner.DAL
{
    internal static class PathConfig
    {
        /// <summary>
        /// The root folder (where all the working files are stored) is set to MyDocuments
        /// Files will be copied and generated into MyDocuments\CoffeeMornings
        /// </summary>
        private const Environment.SpecialFolder MyDocumentsFolder = Environment.SpecialFolder.MyDocuments;

        public static string RootFolder()
        {
            //return Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            var filePath = Environment.GetFolderPath(MyDocumentsFolder);
            filePath = Path.Join(filePath, "CoffeeMornings");
            return filePath;

        }

        public static void ShowRootPath()
        {
            Console.WriteLine("Using this path:");
            Console.WriteLine(RootFolder());
            Console.WriteLine();

        }
    }
}
