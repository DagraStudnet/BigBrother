using System;
using System.Windows.Threading;
using ClassLibrary.UserLibrary;

namespace ClientBigBrother.Model.Monitoring
{
    public class ManagmentMonitoring : IManagmentMonitoring
    {
        private readonly IUserMonitoring<IUser> userMonitoring;

        public ManagmentMonitoring(DispatcherTimer dispatcherTimer)
        {
            PcUser = new User();
            userMonitoring = new UserMonitoring<IUser>();
            StartMonitoring();

            dispatcherTimer.Tick += dispatcherTimer_Tick;
        }

        public bool CanMonitoring { get; set; }

        public IUser PcUser { get; private set; }

        public bool StartMonitoring()
        {
            userMonitoring.SaveStartUpApplicationsOnDestop(PcUser);
            userMonitoring.SaveInformationAboutUserPc(PcUser);
            return CanMonitoring = true;
        }

        private void Monitoring()
        {
            userMonitoring.SaveUsbConnection(PcUser);
            userMonitoring.SaveNowRuningApplicationUser(PcUser);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (CanMonitoring)
                Monitoring();
        }
    }
}