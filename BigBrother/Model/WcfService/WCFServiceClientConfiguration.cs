using System.ServiceModel;

namespace ClientBigBrother.Model.WcfService
{
    public class WcfServiceClientConfiguration
    {
        public WcfServiceClientConfiguration(string address, string timeSend)
        {
            NetTcpBinding = new NetTcpBinding();
            Address = new EndpointAddress(address);
            TimeIntervalInSeconds = int.Parse(timeSend);
        }

        public EndpointAddress Address { get; private set; }
        public NetTcpBinding NetTcpBinding { get; private set; }
        public int TimeIntervalInSeconds { get; private set; }
    }
}