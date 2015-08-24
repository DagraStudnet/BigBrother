using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Xml.Linq;
using ClassLibrary.ConfigFileLibrary;
using ClassLibrary.UserLibrary;
using ClientBigBrother.Annotations;
using ClientBigBrother.Model.Monitoring;
using ClientBigBrother.Model.WcfService;

namespace ClientBigBrother.ViewModel
{
    internal class ViewModelMain : INotifyPropertyChanged
    {
        private readonly CommunicationWithService communicationWithService;
        private ManagmentMonitoring managmentMonitoring;
        private readonly WcfServiceClientConfiguration wcfServiceClientConfiguration;
        private int time;
        private DispatcherTimer timer;
        private bool monitoringStart;
        private bool _hostingIsOnline;

        public bool HostingIsOnline
        {
            get { return _hostingIsOnline; }
            set
            {
                _hostingIsOnline = value;
                if (!monitoringStart && _hostingIsOnline) StartMonitoring();
                OnPropertyChanged();
            }
        }

        public bool ConfigFileDoesntWork { get; set; }

        public ViewModelMain()
        {
            HostingIsOnline = false;
            var confReader = new ConfigFileReader();
            confReader.LoadConfigFile(@"config.xml");
            if (!confReader.XmlDocumentIsEmpty())
            {
                ConfigFileDoesntWork = true;
                return;
            }
            IEnumerable<XElement> elementsCollection = confReader.GetXmlElementsCollection("ConnectionServer");
            ConfigAttribute connectionServerConfigutation = ConnectionServerConfigutation(confReader, elementsCollection);
            wcfServiceClientConfiguration = new WcfServiceClientConfiguration(
                connectionServerConfigutation.Address, connectionServerConfigutation.TimeIntervalInSeconds);
            communicationWithService = new CommunicationWithService(wcfServiceClientConfiguration);
            timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            timer.Tick += dispatcherTimer_Tick;
            timer.Start();
            managmentMonitoring = new ManagmentMonitoring(timer);
        }

        private void StartMonitoring()
        {
            if (!monitoringStart)
                monitoringStart = managmentMonitoring.StartMonitoring();
        }


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
            HostingIsOnline = communicationWithService.HostingIsAlive();
            var dispatcherTimer = sender as DispatcherTimer;
            if (dispatcherTimer != null) time += dispatcherTimer.Interval.Seconds;
            if (wcfServiceClientConfiguration == null) return;
            if (time%wcfServiceClientConfiguration.TimeIntervalInSeconds != 0) return;
            
            if (HostingIsOnline)
            {
                communicationWithService.SendInformationToService(managmentMonitoring.PcUser);
            }
        }

        public void FinishApp()
        {
            if (!HostingIsOnline) return;
            managmentMonitoring.PcUser.ListOfActivitesOnPc.Add(new Activity(){NameActivity = "Close monitoring application",TimeActivity = DateTime.Now});
            communicationWithService.SendInformationToService(managmentMonitoring.PcUser);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}