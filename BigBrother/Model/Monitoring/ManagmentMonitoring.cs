using System;
using System.Windows.Threading;
using ClassLibrary;
using ClientBigBrother.Model.PcUser;

namespace ClientBigBrother.Model.Monitoring
{
    public class ManagmentMonitoring : IManagmentMonitoring
    {
        private readonly IUserMonitoring<IUser> userMonitoring;

        public IUser PcUser { get; private set; }

        public ManagmentMonitoring(DispatcherTimer dispatcherTimer)
        {
            PcUser = new UserContract();
            userMonitoring = new UserMonitoring<IUser>();

            userMonitoring.SaveStartUpApplicationsOnDestop(PcUser);
            userMonitoring.SaveInformationAboutUserPc(PcUser);

            dispatcherTimer.Tick += dispatcherTimer_Tick;
        }

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