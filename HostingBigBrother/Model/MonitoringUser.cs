using System.Collections.Generic;
using ClassLibrary;

namespace HostingBigBrother.Model
{
    public class MonitoringUser : User
    {
        public bool Warning { get; set; }
        public new IList<MonitoringActivity> ListOfActivitesOnPc { get; set; }
    }
}