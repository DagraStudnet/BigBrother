using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ClassLibrary.ConfigFileLibrary;

namespace ClientBigBrother.Model.WcfService
{
    public class LoadConfigurationFile
    {
        private ConfigFileReader confReader;

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
            IEnumerable<XElement> elementsCollection = confReader.GetXmlElementsCollection("ConnectionServer");
            return confReader.GetConfiguration(() => (from n in elementsCollection
                                                      select new ConfigAttribute
                                                      {
                                                          Address = n.Element("Address").Value,
                                                          Port = n.Element("Port").Value,
                                                          ServiceName = n.Element("ServiceName").Value,
                                                          TimeIntervalInSeconds = n.Element("SendingUserInformationTimeIntervalInSeconds").Value
                                                      }).FirstOrDefault());
        }
    }
}
