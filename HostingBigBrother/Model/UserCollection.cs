using System.Collections.Generic;
using System.Linq;
using ClassLibrary.UserLibrary;

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
            IEnumerable<IUser> existUsers = users.Where(user => ExistUser(user.UserName, user.PCName));
            IEnumerable<IUser> noExistUsers = users.Where(user => !ExistUser(user.UserName, user.PCName));

            if (existUsers.Count() != 0)
            {
                foreach (var userExistFromNdb in existUsers)
                {
                    var userFromCollection = FindUser(userExistFromNdb.UserName);

                    var countUserRecordsInCollection = userFromCollection.ListOfActivitesOnPc.Count();
                    var countUserRecordsFromNdb = userExistFromNdb.ListOfActivitesOnPc.Count();

                    if (countUserRecordsInCollection >= countUserRecordsFromNdb) continue;
                    for (var i = countUserRecordsInCollection - 1; i < countUserRecordsFromNdb; i++)
                    {
                        AddUserActivity(userFromCollection, userExistFromNdb.ListOfActivitesOnPc[i]);
                    }
                }
            }

            if (noExistUsers.Count() != 0)
            {
                UserList.AddRange(noExistUsers);
            }
        }

        public void AddUserActivity(IUser user, Activity activity)
        {
            user.ListOfActivitesOnPc.Add(activity);
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