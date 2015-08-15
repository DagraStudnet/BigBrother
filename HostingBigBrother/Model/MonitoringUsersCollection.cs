//using System.Collections.Generic;
//using System.Linq;
//using ClassLibrary.UserLibrary;

//namespace HostingBigBrother.Model
//{
//    public class MonitoringUsersCollection
//    {
//        public MonitoringUsersCollection()
//        {
//            UserList = new List<MonitoringUser>();
//        }

//        public List<MonitoringUser> UserList { get; set; }


//        public void AddRangeUser(IEnumerable<MonitoringUser> users)
//        {
//            IEnumerable<MonitoringUser> existUsers = users.Where(user => ExistUser(user.UserName, user.PCName));
//            IEnumerable<MonitoringUser> noExistUsers = users.Where(user => !ExistUser(user.UserName, user.PCName));

//            if (existUsers.Count() != 0)
//            {
//                foreach (var userExistFromNdb in existUsers)
//                {
//                    var userFromCollection = FindUser(userExistFromNdb.UserName);

//                    var countUserRecordsInCollection = userFromCollection.ListOfActivitesOnPc.Count();
//                    var countUserRecordsFromNdb = userExistFromNdb.ListOfActivitesOnPc.Count();

//                    if (countUserRecordsInCollection >= countUserRecordsFromNdb) continue;
//                    for (var i = countUserRecordsInCollection - 1; i < countUserRecordsFromNdb; i++)
//                    {
//                        AddUserActivity(userFromCollection, userExistFromNdb.ListOfActivitesOnPc[i]);
//                    }
//                }
//            }

//            if (noExistUsers.Count() != 0)
//            {
//                UserList.AddRange(noExistUsers);
//            }
//        }

//        public void AddUserActivity(MonitoringUser user, MonitoringActivity activity)
//        {
//            user.ListOfActivitesOnPc.Add(activity);
//        }

//        private MonitoringUser FindUser(string userName)
//        {
//            return UserList.Find(x => x.UserName.Contains(userName));
//        }

//        public bool ExistUser(string userName, string pcName)
//        {
//            return UserList.Exists(x => x.UserName.Contains(userName) && x.PCName.Contains(pcName));
//        }
//    }
//}