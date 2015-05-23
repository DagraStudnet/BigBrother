using System.Collections.Generic;
using System.Linq;
using ClassLibrary;

namespace HostingBigBrother.Model
{
    public class UserCollection
    {
        public UserCollection()
        {
            UserList = new List<IUser>();
        }

        public List<IUser> UserList { get; set; }

        public void AddUser(IUser user)
        {
            UserList.Add(user);
        }

        public void AddRangeUser(IEnumerable<IUser> users)
        {
            var existUsers = users.Where(user => ExistUser(user.UserName, user.PCName));
            var noExistUsers = users.Where(user => !ExistUser(user.UserName, user.PCName));

            if (existUsers.Count() != 0)
            {
                foreach (IUser user in existUsers)
                {
                    foreach (var activity1 in user.ListOfActivitesOnPc)
                    {
                        var activity = (MonitoringActivity) activity1;
                        AddUserActivity(user.UserName, activity);
                    }
                }
            }

            if (noExistUsers.Count() != 0)
            {
                UserList.AddRange(noExistUsers);
            }
        }

        public void AddUserActivity(string userName, IActivity activity)
        {
            IUser userHelper = FindUser(userName);
            userHelper.ListOfActivitesOnPc.Add((MonitoringActivity)activity);
        }

        private IUser FindUser(string userName)
        {
            return UserList.Find(x => x.UserName.Contains(userName));
        }

        public bool ExistUser(string userName, string pcName)
        {
            return UserList.Exists(x => x.UserName.Contains(userName) && x.PCName.Contains(pcName));
        }
    }
}