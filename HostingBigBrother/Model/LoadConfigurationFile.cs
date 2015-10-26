using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ClassLibrary.ConfigFileLibrary;

namespace BigBrotherViewer.Model
{
    public class LoadConfigurationFile
    {
        private readonly ConfigFileReader confReader;

        public LoadConfigurationFile()
        {
            confReader = new ConfigFileReader();
            confReader.LoadConfigFile(@"config.xml");
        }

        public bool IsExistConfigFile()
        {
            return confReader.XmlDocumentIsEmpty();
        }

        public ConfigAttribute ConnectionServerConfigutation()
        {
            IEnumerable<XElement> elementsCollection = confReader.GetXmlElementsCollection("ServerConfiguration");
            return confReader.GetConfiguration(() => (from n in elementsCollection
                select new ConfigAttribute
                {
                    TimeIntervalInSeconds = n.Element("RefreshTimeIntervalInSeconds").Value
                }).FirstOrDefault());
        }
    }
}