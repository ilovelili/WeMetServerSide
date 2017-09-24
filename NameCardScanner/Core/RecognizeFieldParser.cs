using System.Linq;
using System.Xml.Linq;
using NamecardScanner.Models;

namespace NamecardScanner.Core
{
    public static class RecognizeFieldParser
    {
        public static RecognizeResponse ParseRecognizeResponse(string xmlFilePath)
        {
            var xDoc = XDocument.Load(xmlFilePath);
            var descendants = xDoc.Descendants().ToList();

            if (!descendants.Any()) return null;

            var phone = descendants.FirstOrDefault(x => (string)x.Attribute("type") == "Phone");
            var name = descendants.FirstOrDefault(x => (string)x.Attribute("type") == "Name");
            var company = descendants.FirstOrDefault(x => (string)x.Attribute("type") == "Company");
            var address = descendants.FirstOrDefault(x => (string)x.Attribute("type") == "Address");
            var email = descendants.FirstOrDefault(x => (string)x.Attribute("type") == "Email");
            var job = descendants.FirstOrDefault(x => (string)x.Attribute("type") == "Job");
            var web = descendants.FirstOrDefault(x => (string)x.Attribute("type") == "Web");
            var text = descendants.FirstOrDefault(x => (string)x.Attribute("type") == "Text");

            return new RecognizeResponse()
            {
                Name = name?.Value,
                Company = company?.Value,
                Email = email?.Value,
                Job = job?.Value,
                Phone = phone?.Value,
                Text = text?.Value,
                Web = web?.Value,
            };
        }
    }
}
