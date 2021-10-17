using System;
using System.IO;
using Newtonsoft.Json;

namespace CoffeeMorningAssigner.DAL
{
    public class JsonRepository : Repository
    {
        public T Load<T>(string filename) where T : new()
        {
            var filePath = FilePath(filename);

            Console.WriteLine($"Loading data from {filePath}");

            if (File.Exists(filePath))
            {
                var input = File.ReadAllText(filePath);

                if (!string.IsNullOrEmpty(input))
                {
                    return JsonConvert.DeserializeObject<T>(input);
                }
            }


            return new T();
        }

        public void Save(string filename, object value)
        {
            var filePath = FilePath(filename);
            var output = JsonConvert.SerializeObject(value);
            File.WriteAllText(filePath, output);
        }
    }
}