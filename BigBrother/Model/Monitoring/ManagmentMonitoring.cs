using System;
using System.Windows.Threading;
using ClassLibrary;

namespace ClientBigBrother.Model.Monitoring
{
    public class ManagmentMonitoring : IManagmentMonitoring
    {
        private readonly IUserMonitoring<IUser> userMonitoring;

        public ManagmentMonitoring(DispatcherTimer dispatcherTimer)
        {
            PcUser = new User();
            userMonitoring = new UserMonitoring<IUser>();

            userMonitoring.SaveStartUpApplicationsOnDestop(PcUser);
            userMonitoring.SaveInformationAboutUserPc(PcUser);

            dispatcherTimer.Tick += dispatcherTimer_Tick;
        }

        public IUser PcUser { get; private set; }

        private void Monitoring()
        {
            userMonitoring.SaveUsbConnection(PcUser);
            userMonitoring.SaveNowRuningApplicationUser(PcUser);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Monitoring();
        }
    }
}