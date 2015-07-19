using System;
using System.Linq;
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
            var exceptedActivity = "Microsoft Visual Studio";
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1); //interval v sekundach
            var managment = new ManagmentMonitoring(dispatcherTimer);
            dispatcherTimer.Start();
            Assert.AreNotSame(managment.PcUser.ListOfActivitesOnPc.Count,0);
            var activity = managment.PcUser.ListOfActivitesOnPc.First(x => x.NameActivity.Contains(exceptedActivity));
            Assert.AreNotEqual(activity,null);
        }
        
    }
}
