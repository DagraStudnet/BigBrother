using System;
using System.Windows.Threading;
using ClientBigBrother.Model.Monitoring;
using ClientBigBrother.Model.WcfService;

namespace ClientBigBrother.ViewModel
{
    internal class ViewModelMain
    {
        private readonly CommunicationWithService communicationWithService;
        private readonly IManagmentMonitoring managmentMonitoring;
        private int time;
        private DispatcherTimer timer;

        public ViewModelMain()
        {
            timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 1)};
            timer.Tick += dispatcherTimer_Tick;
            timer.Start();
            communicationWithService = new CommunicationWithService();
            managmentMonitoring = new ManagmentMonitoring(timer);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var dispatcherTimer = sender as DispatcherTimer;
            if (dispatcherTimer != null) time += dispatcherTimer.Interval.Seconds;
            if (time%60 == 0)
                communicationWithService.SendInformationToService(managmentMonitoring.PcUser);
        }
    }
}