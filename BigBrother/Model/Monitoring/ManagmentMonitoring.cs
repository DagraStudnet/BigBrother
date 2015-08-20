using System;
using System.Windows.Threading;
using ClassLibrary;
using ClassLibrary.UserLibrary;

namespace ClientBigBrother.Model.Monitoring
{
    public class ManagmentMonitoring : IManagmentMonitoring
    {
        private readonly IUserMonitoring<IUser> userMonitoring;
        private bool CanMonitoring { get; set; }

        public ManagmentMonitoring(DispatcherTimer dispatcherTimer)
        {
            PcUser = new User();
            userMonitoring = new UserMonitoring<IUser>();
            StartMonitoring();

            dispatcherTimer.Tick += dispatcherTimer_Tick;
        }

        public bool StartMonitoring()
        {
            userMonitoring.SaveStartUpApplicationsOnDestop(PcUser);
            userMonitoring.SaveInformationAboutUserPc(PcUser);
            CanMonitoring = true;
            return true;
        }

        public IUser PcUser { get; private set; }

        private void Monitoring()
        {
            userMonitoring.SaveUsbConnection(PcUser);
            userMonitoring.SaveNowRuningApplicationUser(PcUser);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if(CanMonitoring)
            Monitoring();
        }
    }
}