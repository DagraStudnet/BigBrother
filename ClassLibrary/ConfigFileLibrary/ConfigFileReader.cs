using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ClassLibrary.ConfigFileLibrary
{
    public class ConfigFileReader
    {
        private XDocument xmlDocument;


        public void LoadConfigFile(string pathXmlDocument)
        {
            try
            {
                xmlDocument = XDocument.Load(pathXmlDocument);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                xmlDocument = null;
            }
        }
        
        public IEnumerable<XElement> GetXmlElementsCollection(string xmlNameAttribute)
        {
            return xmlDocument.Descendants(xmlNameAttribute);
        }

        public ConfigAttribute GetConfiguration(Func<ConfigAttribute> func)
        {
            return func();
        }

        public bool XmlDocumentIsEmpty()
        {
            return xmlDocument != null;
        }
    }
}