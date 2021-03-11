using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace PDF.Demo.Api.Test.Business
{
    public class BaseTestClass : IDisposable
    {
        private const string SampleDataPath = "./Business/SampleData";

        public JObject GetJObjectFromFile(string fileName)
        {
            var data = JObject.Parse(this.GetTextFromFile(fileName));

            return data;
        }

        public string GetTextFromFile(string fileName)
        {
            var textFromFile = File.ReadAllText($"{SampleDataPath}/{fileName}");

            return textFromFile;
        }

        public virtual void Dispose()
        {

        }
    }
}
