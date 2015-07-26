using System;
using System.ServiceModel;
using System.Windows;
using ClassLibrary;
using ClientBigBrother.WcfServiceLibrary;

namespace ClientBigBrother.Model.WcfService
{
    public class CommunicationWithService
    {
        private LibraryClient proxy;

        public void SendInformationToService(IUser user)
        {
            try
            {
                //WcfService<LibraryClient>.AutoRepair(ref proxy);
                using (proxy = new LibraryClient())
                {
                    if (proxy.State == CommunicationState.Faulted)
                        throw new CommunicationObjectFaultedException("Connection fault.");
                    if (proxy.State == CommunicationState.Closed)
                        proxy.Open();
                    proxy.AddUser((User)user);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                proxy.Abort();
            }
        }
    }
}