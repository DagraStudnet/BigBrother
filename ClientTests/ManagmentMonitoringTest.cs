using System;
using System.Windows.Threading;
using ClientBigBrother.Model.Monitoring;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientTests
{
    [TestClass]
    public class ManagmentMonitoringTest
    {
        [TestMethod]
        public void Should_return_user_Activites()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1); //interval v sekundach
            ManagmentMonitoring managment = new ManagmentMonitoring(dispatcherTimer);
            dispatcherTimer.Start();
            Assert.AreNotSame(managment.PcUser.ListOfActivitesOnPc.Count,0); 
        }
    }
}
