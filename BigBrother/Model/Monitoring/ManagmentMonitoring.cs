using System;
using System.Windows.Threading;
using ClassLibrary;
using ClientBigBrother.Model.PcUser;

namespace ClientBigBrother.Model.Monitoring
{
    public class ManagmentMonitoring : IManagmentMonitoring
    {
        private readonly IUserMonitoring<IUser> userMonitoring;

        public ManagmentMonitoring(DispatcherTimer dispatcherTimer)
        {
            User = new UserContract();
            userMonitoring = new UserMonitoring<IUser>();

            userMonitoring.SaveStartUpProgramsOnDestop(User);
            userMonitoring.SaveInformationAboutUserPc(User);

            dispatcherTimer.Tick += dispatcherTimer_Tick;
        }

        public IUser User { get; set; }

        private void Monitoring()
        {
            userMonitoring.SaveUsbConnection(User);
            userMonitoring.SaveStartUpProgramsOnDestop(User);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Monitoring();
        }
    }
}