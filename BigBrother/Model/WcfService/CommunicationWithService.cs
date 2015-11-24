using System;
using System.ServiceModel;
using System.Windows;
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

        public bool HostingIsAlive()
        {
            try
            {
                proxy = ConnectionProxy();
                var isAlive = proxy.IsAlive();
                return isAlive;
            }
            catch (EndpointNotFoundException e)
            {
                proxy.Abort();
                proxy = null;
                return false;
            }
        }

        public void SendInformationToService(IUser user)
        {
            try
            {
                if (proxy == null)
                    proxy = ConnectionProxy();
                if (proxy.State == CommunicationState.Faulted)
                    throw new CommunicationObjectFaultedException("Connection fault.");
                if (proxy.State == CommunicationState.Closed)
                    proxy.Open();
                user.TimeStampDispatch = DateTime.Now;
                proxy.AddUser((User)user);
                user.ListOfActivitesOnPc.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                proxy.Abort();
                proxy = null;
            }
        }

        private LibraryClient ConnectionProxy()
        {
            return new LibraryClient(
                wcfServiceClientConfiguration.NetTcpBinding, wcfServiceClientConfiguration.Address);
        }
    }
}