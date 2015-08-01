using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;
using NUnit.Framework;

namespace UnitTestProject1
{
    public static class UserStorageHelper
    {
         public const string ExeptedPcName = "NB17";
         public const string ExeptedUserName = "Lukas";

        public static IUser AddActivites(IUser userFromStorage)
        {
            var newActivitiesList = new List<Activity>();
            newActivitiesList.AddRange(userFromStorage.ListOfActivitesOnPc);

            for (int i = 1; i < 4; i++)
            {
                newActivitiesList.Add(new Activity
                {
                    NameActivity = "Name activity" + i,
                    TimeActivity = new DateTime(2005, 1, 1, 1, 1, 0).AddSeconds(i)
                });
            }

            userFromStorage.ListOfActivitesOnPc = newActivitiesList;
            return userFromStorage;
        }

        public static void FindUsers(IEnumerable<IUser> usersFromStorage)
        {
            for (int i = 0; i < usersFromStorage.Count(); i++)
            {
                IUser userFromStorage = GetUserFromStorage(usersFromStorage, ExeptedUserName + i, ExeptedPcName + i);
                Assert.AreNotEqual(null, userFromStorage);
                Assert.AreEqual(ExeptedPcName + i, userFromStorage.PCName);
                Assert.AreEqual(ExeptedUserName + i, userFromStorage.UserName);
            }
        }

        public static IEnumerable<IUser> GetUsers()
        {
            var userList = new List<IUser> {GetUser(1), GetUser(2), GetUser(3)};
            return userList;
        }

        public static IUser GetUserFromStorage(IEnumerable<IUser> usersFromStorage, string exeptedUserName,
            string exeptedPcName)
        {
            return
                usersFromStorage.ToList()
                    .Find(u => u.UserName.Equals(exeptedUserName) && u.PCName.Equals(exeptedPcName));
        }

        public static void AssertActivites(IUser userFromStorage, int startIndex=0)
        {
            for (var index = startIndex; index < userFromStorage.ListOfActivitesOnPc.Count; index++)
            {
                Activity activity = userFromStorage.ListOfActivitesOnPc[index];
                Assert.AreNotEqual(null, activity);
                Assert.AreEqual("Name activity" + index, activity.NameActivity);
                Assert.AreEqual(new DateTime(2005, 1, 1, 1, 1, 0).AddSeconds(index), activity.TimeActivity);
            }
        }

        public static IUser GetUser(int id)
        {
            return new User
            {
                PCName = "NB17" + id,
                UserName = "Lukas" + id,
                TimeStampsDispatch = DateTime.Now,
                ListOfActivitesOnPc =
                    new[]
                    {
                        new Activity
                        {
                            NameActivity = "Name activity" + id,
                            TimeActivity = new DateTime(2005, 1, 1, 1, 1, 0)
                        }
                    }
            };
        }
    }
}