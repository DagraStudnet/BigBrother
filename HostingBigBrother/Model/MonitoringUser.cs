using System.Collections.Generic;
using ClassLibrary;
using ClassLibrary.UserLibrary;

namespace HostingBigBrother.Model
{
    public class MonitoringUser : User
    {
        public bool Attention { get; set; }
        public new IList<MonitoringActivity> ListOfActivitesOnPc { get; set; }
    }
}