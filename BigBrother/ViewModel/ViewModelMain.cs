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
        private readonly ManagmentMonitoring managmentMonitoring;
        private readonly WcfServiceClientConfiguration wcfServiceClientConfiguration;
        private int time;
        private readonly DispatcherTimer timer;
        private bool monitoringStart;
        private bool _hostingIsOnline;
        private readonly BackgroundWorker backgroundWorkerServiceIsAlive;
        

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
            backgroundWorkerServiceIsAlive = new BackgroundWorker();
            HostingIsOnline = false;
            var configuration = new LoadConfigurationFile();
            if (!configuration.IsExistConfigFile())
            {
                ConfigFileDoesntWork = true;
                return;
            }

            ConfigAttribute connectionServerConfigutation = configuration.ConnectionServerConfigutation();
            wcfServiceClientConfiguration = new WcfServiceClientConfiguration(connectionServerConfigutation);
            communicationWithService = new CommunicationWithService(wcfServiceClientConfiguration);
            timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            timer.Tick += dispatcherTimer_Tick;
            timer.Start();
            managmentMonitoring = new ManagmentMonitoring();
        }

        private void StartMonitoring()
        {
            if (!monitoringStart)
                monitoringStart = managmentMonitoring.StartMonitoring();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!backgroundWorkerServiceIsAlive.IsBusy)
            {
                backgroundWorkerServiceIsAlive.DoWork += BackgroundWorkerServiceIsAliveOnDoWork;
                backgroundWorkerServiceIsAlive.RunWorkerAsync();
            }
            var dispatcherTimer = sender as DispatcherTimer;
            if (dispatcherTimer != null) time += dispatcherTimer.Interval.Seconds;
            if (wcfServiceClientConfiguration == null) return;
            if (time % wcfServiceClientConfiguration.TimeIntervalInSeconds != 0) return;
            if (!HostingIsOnline) return;
            communicationWithService.SendInformationToService(managmentMonitoring.PcUser);
        }

        private void BackgroundWorkerServiceIsAliveOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            HostingIsOnline = communicationWithService.HostingIsAlive();
        }

        public void FinishApp()
        {
            timer.Stop();
            if (!HostingIsOnline) return;
            managmentMonitoring.PcUser.ListOfActivitesOnPc.Add(new Activity() { NameActivity = "Close monitoring application", TimeActivity = DateTime.Now });
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