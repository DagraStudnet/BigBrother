using System;
using System.ServiceModel;

namespace ClientBigBrother.Model.WcfService
{
    public class WcfServiceClientConfiguration
    {
        public WcfServiceClientConfiguration(string address, string timeSend)
        {
            NetTcpBinding = new NetTcpBinding(SecurityMode.None);
            Address = new EndpointAddress(address);
            NetTcpBinding.ReceiveTimeout = TimeSpan.FromMinutes(5);
            NetTcpBinding.OpenTimeout = TimeSpan.FromMinutes(5);
            NetTcpBinding.MaxReceivedMessageSize = 2147483647;
            NetTcpBinding.MaxBufferSize = 2147483647;
            NetTcpBinding.MaxBufferPoolSize = 2147483647;
            NetTcpBinding.TransferMode = TransferMode.Buffered;
            TimeIntervalInSeconds = int.Parse(timeSend);
        }

        public EndpointAddress Address { get; private set; }
        public NetTcpBinding NetTcpBinding { get; private set; }
        public int TimeIntervalInSeconds { get; private set; }
    }
}