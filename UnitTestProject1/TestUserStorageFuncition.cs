using System;
using System.Collections.Generic;
using ClassLibrary.UserLibrary;
using NDatabase;
using NDatabase.Api;
using NUnit.Framework;
using UserStorageNDatabase;

namespace UnitTestProject1
{
    [TestFixture]
    internal class TestUserStorageFuncition
    {
        [SetUp]
        public void Init()
        {
        }

        private IOdb odb;
        private IEnumerable<IUser> usersFromDb;
        private const string dbName = "testFunction.ndb";

        [Test]
        public void Find_user_in_the_storage()
        {
            OdbFactory.Delete(dbName);
            using (odb = OdbFactory.Open(dbName))
            {
                IUser expectedUser = UserStorageHelper.GetUser(1);
                IEnumerable<IUser> users = UserStorageHelper.GetUsers();
                foreach (IUser user in users)
                {
                    UserStorage.SaveUserToStore(odb, user);
                }
                IUser findUser = UserStorage.FindUser(odb, expectedUser);
                Assert.AreEqual(expectedUser.PCName, findUser.PCName);
                Assert.AreEqual(expectedUser.UserName, findUser.UserName);

                expectedUser = UserStorageHelper.GetUser(2);
                findUser = UserStorage.FindUser(odb, expectedUser);
                Assert.AreEqual(expectedUser.PCName, findUser.PCName);
                Assert.AreEqual(expectedUser.UserName, findUser.UserName);
            }
        }

        [Test]
        public void Save_user_to_storage()
        {
            OdbFactory.Delete(dbName);
            using (odb = OdbFactory.Open(dbName))
            {
                IUser user = UserStorageHelper.GetUser(0);
                UserStorage.SaveUserToStore(odb, user);
                usersFromDb = odb.QueryAndExecute<IUser>();
                UserStorageHelper.FindUsers(usersFromDb);
            }
        }

        [Test]
        public void Set_user_new_timestamp_in_the_storage()
        {
            OdbFactory.Delete(dbName);
            using (odb = OdbFactory.Open(dbName))
            {
                IUser expectedUser = UserStorageHelper.GetUser(1);
                IEnumerable<IUser> users = UserStorageHelper.GetUsers();
                foreach (IUser user in users)
                {
                    UserStorage.SaveUserToStore(odb, user);
                }
                IUser findUser = UserStorage.FindUser(odb, expectedUser);
                DateTime oldTimeStamp = findUser.TimeStampsDispatch;
                findUser.TimeStampsDispatch = DateTime.Now;
                DateTime newTimeStamp = findUser.TimeStampsDispatch;
                odb.Store(findUser);
                findUser = UserStorage.FindUser(odb, expectedUser);
                Assert.AreEqual(newTimeStamp, findUser.TimeStampsDispatch);
                Assert.Greater(findUser.TimeStampsDispatch, oldTimeStamp);
            }
        }

        [Test]
        public void Update_new_list_activites_for_user_in_the_storage()
        {
            OdbFactory.Delete(dbName);
            using (odb = OdbFactory.Open(dbName))
            {
                IUser updatedUser = UserStorageHelper.GetUser(1);
                updatedUser.ListOfActivitesOnPc = new Activity[0];
                UserStorageHelper.AddActivites(updatedUser);
                IEnumerable<IUser> users = UserStorageHelper.GetUsers();
                foreach (IUser user in users)
                {
                    UserStorage.SaveUserToStore(odb, user);
                }
                IUser findUser = UserStorage.FindUser(odb, updatedUser);
                UserStorage.SetActivitiesToExistUser(updatedUser, findUser);
                odb.Store(findUser);
                findUser = UserStorage.FindUser(odb, updatedUser);
                Assert.AreEqual(4, findUser.ListOfActivitesOnPc.Count);
                UserStorageHelper.AssertActivites(findUser, 1);
            }
        }
    }
}