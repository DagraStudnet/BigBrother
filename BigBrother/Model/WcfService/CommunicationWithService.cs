using System;
using System.ServiceModel;
using System.Windows;
using ClassLibrary;
using ClassLibrary.UserLibrary;
using ClientBigBrother.WcfServiceLibrary;

namespace ClientBigBrother.Model.WcfService
{
    public class CommunicationWithService
    {
        private LibraryClient proxy;
        private readonly WcfServiceClientConfiguration wcfServiceClientConfiguration;

        public CommunicationWithService(WcfServiceClientConfiguration wcfServiceClientConfiguration)
        {
            this.wcfServiceClientConfiguration = wcfServiceClientConfiguration;
            
        }

        public void SendInformationToService(IUser user)
        {
            try
            {                
                proxy = new LibraryClient(wcfServiceClientConfiguration.NetTcpBinding,wcfServiceClientConfiguration.Address);
                if (proxy.State == CommunicationState.Faulted)
                    throw new CommunicationObjectFaultedException("Connection fault.");
                if (proxy.State == CommunicationState.Closed)
                    proxy.Open();
                proxy.AddUser((User) user);
                user.ListOfActivitesOnPc.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                proxy.Abort();
            }
        }
    }
}