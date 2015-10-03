using System;
using System.ServiceModel;
using ClassLibrary.ConfigFileLibrary;

namespace ClientBigBrother.Model.WcfService
{
    public class WcfServiceClientConfiguration
    {
        public WcfServiceClientConfiguration(ConfigAttribute connectionServerConfigutation)
        {
            var address = string.Format("{0}:{1}/{2}", connectionServerConfigutation.Address,
                connectionServerConfigutation.Port, connectionServerConfigutation.ServiceName);
            NetTcpBinding = new NetTcpBinding(SecurityMode.None);
            Address = new EndpointAddress(address);
            NetTcpBinding.ReceiveTimeout = TimeSpan.FromMilliseconds(100);
            NetTcpBinding.SendTimeout = TimeSpan.FromMilliseconds(100);
            NetTcpBinding.OpenTimeout = TimeSpan.FromMilliseconds(100);
            NetTcpBinding.CloseTimeout = TimeSpan.FromMilliseconds(100);
            
            NetTcpBinding.MaxReceivedMessageSize = 2147483647;
            NetTcpBinding.MaxBufferSize = 2147483647;
            NetTcpBinding.MaxBufferPoolSize = 2147483647;
            NetTcpBinding.TransferMode = TransferMode.Buffered;
            TimeIntervalInSeconds = int.Parse(connectionServerConfigutation.TimeIntervalInSeconds);
        }
        

        public EndpointAddress Address { get; private set; }
        public NetTcpBinding NetTcpBinding { get; private set; }
        public int TimeIntervalInSeconds { get; private set; }
    }
}