using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;
using NDatabase;
using NDatabase.Api;

namespace UserStorageNDatabase
{
    public class UserStorage
    {
        private const string NameNDatabase = "user_storage.ndb";
        private static UserStorage instance;

        private UserStorage()
        {
            OdbFactory.Delete(NameNDatabase);
        }

        public static UserStorage ReturnDatabaseInstance()
        {
            return instance ?? (instance = new UserStorage());
        }

        public void AddUserToDbStorage(IUser user)
        {
            using (IOdb odb = OdbFactory.Open(NameNDatabase))
            {
                IUser dbUser = FindUser(odb, user);
                if (dbUser == null)
                {
                    SetTimeStamps(user);
                    SaveUserToStore(odb, user);
                }
                else
                {
                    SetTimeStamps(dbUser);
                    SetActivitiesToExistUser(user, dbUser);
                    SaveUserToStore(odb, dbUser);
                }
            }
        }

        public static void SetActivitiesToExistUser(IUser user, IUser dbUser)
        {
            var activityList = new List<Activity>();
            activityList.AddRange(dbUser.ListOfActivitesOnPc);
            activityList.AddRange(user.ListOfActivitesOnPc);
            dbUser.ListOfActivitesOnPc = activityList;
        }

        public static void SetTimeStamps(IUser user)
        {
            user.TimeStampsDispatch = DateTime.Now;
        }

        public static void  SaveUserToStore(IOdb odb, IUser dbUser)
        {
             odb.Store(dbUser);
        }

        public static IUser FindUser(IOdb odb, IUser user)
        {
            return
                odb.QueryAndExecute<IUser>()
                    .ToList()
                    .Find(odbUser => odbUser.PCName.Equals(user.PCName) && odbUser.UserName.Equals(user.UserName));

        }

        public IEnumerable<IUser> GetCollectionUsersFromDb()
        {
            using (IOdb odb = OdbFactory.Open(NameNDatabase))
            {
                return odb.QueryAndExecute<User>();
            }
        }
    }
}