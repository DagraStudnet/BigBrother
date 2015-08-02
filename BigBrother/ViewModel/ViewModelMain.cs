using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using System.Xml.Linq;
using ClassLibrary.ConfigFileLibrary;
using ClientBigBrother.Model.Monitoring;
using ClientBigBrother.Model.WcfService;

namespace ClientBigBrother.ViewModel
{
    internal class ViewModelMain
    {
        private readonly CommunicationWithService communicationWithService;
        private readonly IManagmentMonitoring managmentMonitoring;
        private readonly WcfServiceClientConfiguration wcfServiceClientConfiguration;
        private int time;
        private DispatcherTimer timer;

        public ViewModelMain()
        {
            var confReader = new ConfigFileReader();
            confReader.LoadConfigFile(@"config.xml");
            if (!confReader.XmlDocumentIsEmpty())
            {
                ConfigFileDoesntWork = true;
                return;
            }
            timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 1)};
            timer.Tick += dispatcherTimer_Tick;
            timer.Start();
            IEnumerable<XElement> elementsCollection = confReader.GetXmlElementsCollection("ConnectionServer");
            ConfigAttribute connectionServerConfigutation = ConnectionServerConfigutation(confReader, elementsCollection);
            wcfServiceClientConfiguration = new WcfServiceClientConfiguration(
                connectionServerConfigutation.Address, connectionServerConfigutation.TimeIntervalInSeconds);
            communicationWithService = new CommunicationWithService(wcfServiceClientConfiguration);
            managmentMonitoring = new ManagmentMonitoring(timer);
        }

        public bool ConfigFileDoesntWork { get; set; }

        private static ConfigAttribute ConnectionServerConfigutation(ConfigFileReader confReader,
            IEnumerable<XElement> elementsCollection)
        {
            return confReader.GetConfiguration(() => (from n in elementsCollection
                select new ConfigAttribute
                {
                    Address = n.Element("Address").Value,
                    TimeIntervalInSeconds = n.Element("TimeIntervalInSeconds").Value
                }).FirstOrDefault());
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var dispatcherTimer = sender as DispatcherTimer;
            if (dispatcherTimer != null) time += dispatcherTimer.Interval.Seconds;
            if (wcfServiceClientConfiguration == null) return;
            if (time%wcfServiceClientConfiguration.TimeIntervalInSeconds == 0)
                communicationWithService.SendInformationToService(managmentMonitoring.PcUser);
        }
    }
}