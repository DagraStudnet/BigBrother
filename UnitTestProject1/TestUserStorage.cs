using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;
using NUnit.Framework;
using UserStorageNDatabase;

namespace UnitTestProject1
{
    [TestFixture]
    public class TestUserStorage
    {
        [SetUp]
        public void Init()
        {
            userStorage = UserStorage.ReturnDatabaseInstance();
        }
        
        private UserStorage userStorage;        

        [Test]
        public void Add_user_to_storage()
        {
            IUser user = UserStorageHelper.GetUser(0);
            userStorage.AddUserToDbStorage(user);
            IEnumerable<IUser> usersFromStorage = userStorage.GetCollectionUsersFromDb();
            UserStorageHelper.FindUsers(usersFromStorage);
        }

        [Test]
        public void Add_users_to_storage()
        {
            IEnumerable<IUser> addUsers = UserStorageHelper.GetUsers();
            foreach (IUser addUser in addUsers)
            {
                userStorage.AddUserToDbStorage(addUser);
            }
            IEnumerable<IUser> usersFromStorage = userStorage.GetCollectionUsersFromDb();
            Assert.AreEqual(4, usersFromStorage.Count());
            UserStorageHelper.FindUsers(usersFromStorage);
        }

        [Test]
        public void Update_user_activities_on_storage()
        {
            IEnumerable<IUser> usersFromStorage = userStorage.GetCollectionUsersFromDb();
            Assert.AreEqual(4, usersFromStorage.Count());
            UserStorageHelper.FindUsers(usersFromStorage);

            IUser userFromStorage = UserStorageHelper.GetUserFromStorage(usersFromStorage, UserStorageHelper.ExeptedUserName + 0, UserStorageHelper.ExeptedPcName + 0);
            IUser updatedUser = UserStorageHelper.AddActivites(userFromStorage);
            userStorage.AddUserToDbStorage(updatedUser);

            Assert.AreEqual(4, usersFromStorage.Count());
            UserStorageHelper.FindUsers(usersFromStorage);
            userFromStorage = UserStorageHelper.GetUserFromStorage(usersFromStorage, UserStorageHelper.ExeptedUserName + 0, UserStorageHelper.ExeptedPcName + 0);
            UserStorageHelper.AssertActivites(userFromStorage);
        }
    }
}