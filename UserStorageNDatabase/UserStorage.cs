using System;
using System.Collections.Generic;
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
                    SaveObjectToStore(odb, user);
                }
                else
                {
                    SetTimeStamps(dbUser);
                    foreach (Activity activity in user.ListOfActivitesOnPc)
                    {
                        dbUser.ListOfActivitesOnPc.Add(activity);
                    }
                    SaveObjectToStore(odb, dbUser);
                }
            }
        }

        private static void SetTimeStamps(IUser user)
        {
            user.TimeStampsDispatch = DateTime.Now;
        }

        private static OID SaveObjectToStore(IOdb odb, IUser dbUser)
        {
            return odb.Store(dbUser);
        }

        private IUser FindUser(IOdb odb, IUser user)
        {
            OID oid = odb.GetObjectId(user);
            return oid.ObjectId != -1 ? (IUser) odb.GetObjectFromId(oid) : null;
        }

        public IEnumerable<IUser> GetCollectionUsersFromDB()
        {
            using (IOdb odb = OdbFactory.Open(NameNDatabase))
            {
                return odb.QueryAndExecute<User>();
            }
        }
    }
}