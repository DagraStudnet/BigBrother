using System.Collections.Generic;
using System.Linq;
using ClassLibrary;
using HostingBigBrother.Model;

namespace HostingBigBrother.ViewModel
{
    public static class TransformerUsersFromDbStorage
    {
        public static MonitoringUser UserTransform(IUser user)
        {
            return new MonitoringUser
            {
                UserName = user.UserName,
                PCName = user.PCName,
                TimeStampsDispatch = user.TimeStampsDispatch,
                ListOfActivitesOnPc = ActivityTransform(user)
            };
        }

        public static List<MonitoringActivity> ActivityTransform(IUser user)
        {
            return
                (user.ListOfActivitesOnPc.Select(
                    activity =>
                        new MonitoringActivity
                        {
                            NameActivity = activity.NameActivity,
                            TimeActivity = activity.TimeActivity
                        })).ToList();
        }
    }
}