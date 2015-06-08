using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Threading;
using ClientBigBrother.Model;
using ClientBigBrother.WcfServiceLibrary;
using WcfServiceLibrary;

namespace ClientBigBrother.ViewModel
{
    internal class ViewModelMain
    {
        private readonly MananingUser mananingUser;
        private int time;
        private int numberTries = 0;
        private LibraryClient proxy;

        public ViewModelMain()
        {
            mananingUser = new MananingUser();
            mananingUser.StartUpApplication();

            //DataContext = mananingUser.UserContractPc.ListOfActivitesOnPc;

            
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var dispatcherTimer = sender as DispatcherTimer;
            if (dispatcherTimer != null) time += dispatcherTimer.Interval.Seconds;
            MonitoringUser();
            if(time % 60 == 0)
                SendInformationAboutUser();
        }

        private void MonitoringUser()
        {
            mananingUser.SaveConnectionUsb();
            mananingUser.SaveUserActivities();
        }

        public void SendInformationAboutUser()
        {
            try
            {
                if (proxy == null)
                    proxy = new LibraryClient();
                //while (proxy.State != CommunicationState.Faulted)
                //{
                //    ReconnectService(ref proxy);
                //}
                if (proxy.State != CommunicationState.Closed || proxy.State != CommunicationState.Faulted)
                {
                    proxy.AddUser(mananingUser.User);
                    mananingUser.ClearUserActivites();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                proxy = null;
            }
        }

        private static void ReconnectService(ref LibraryClient libraryClient)
        {
            WcfService<LibraryClient>.AutoRepair(ref libraryClient);
        }
    }
}