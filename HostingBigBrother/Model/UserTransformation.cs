using System;
using System.Collections.Generic;
using System.Linq;

namespace HostingBigBrother.Model
{
    public static class UserTransformation
    {
        public static MonitoringUser TransformUser(Db_user dbUser)
        {
            return new MonitoringUser
            {
                Attention = false,
                PCName = dbUser.user_name,
                UserName = dbUser.user_name,
                //TimeStampDispatch = 
                ListOfActivitesOnPc = new List<MonitoringActivity>(dbUser.Db_activity.Select(a => new MonitoringActivity
                {
                    NameActivity = a.name,
                    TimeActivity = DateTime.Parse(a.time_activity),
                    ActivitySearch = false
                }))
            };
        }
    }
}