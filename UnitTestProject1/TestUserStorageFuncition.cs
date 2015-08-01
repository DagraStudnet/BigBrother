using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using NDatabase;
using NDatabase.Api;
using NUnit.Framework;
using UserStorageNDatabase;

namespace UnitTestProject1
{
    [TestFixture]
    class TestUserStorageFuncition
    {
        private IOdb odb;
        private IEnumerable<IUser> usersFromDb;
        private const string dbName = "testFunction.ndb";

        [SetUp]
        public void Init()
        {
        }

        [Test]
        public void Save_user_to_storage()
        {
            OdbFactory.Delete(dbName);
            using (odb = OdbFactory.Open(dbName))
            {
                var user = UserStorageHelper.GetUser(0);
                UserStorage.SaveUserToStore(odb, user);
                usersFromDb = odb.QueryAndExecute<IUser>();
                UserStorageHelper.FindUsers(usersFromDb);
            }
        }

        [Test]
        public void Find_user_in_the_storage()
        {
            OdbFactory.Delete(dbName);
            using (odb = OdbFactory.Open(dbName))
            {
                var expectedUser = UserStorageHelper.GetUser(1);
                var users = UserStorageHelper.GetUsers();
                foreach (var user in users)
                {
                    UserStorage.SaveUserToStore(odb, user);
                }
                var findUser = UserStorage.FindUser(odb,expectedUser);
                Assert.AreEqual(expectedUser.PCName,findUser.PCName);
                Assert.AreEqual(expectedUser.UserName,findUser.UserName);

                expectedUser = UserStorageHelper.GetUser(2);
                findUser = UserStorage.FindUser(odb, expectedUser);
                Assert.AreEqual(expectedUser.PCName, findUser.PCName);
                Assert.AreEqual(expectedUser.UserName, findUser.UserName);
            }
        }

        [Test]
        public void Set_user_new_timestamp_in_the_storage()
        {
            OdbFactory.Delete(dbName);
            using (odb = OdbFactory.Open(dbName))
            {
                var expectedUser = UserStorageHelper.GetUser(1);
                var users = UserStorageHelper.GetUsers();
                foreach (var user in users)
                {
                    UserStorage.SaveUserToStore(odb, user);
                }
                var findUser = UserStorage.FindUser(odb, expectedUser);
                var oldTimeStamp = findUser.TimeStampsDispatch;
                findUser.TimeStampsDispatch =  DateTime.Now;
                var newTimeStamp = findUser.TimeStampsDispatch;
                odb.Store(findUser);
                findUser = UserStorage.FindUser(odb, expectedUser);
                Assert.AreEqual(newTimeStamp, findUser.TimeStampsDispatch);
                Assert.Greater(findUser.TimeStampsDispatch,oldTimeStamp);
            }
        }

        [Test]
        public void Update_new_list_activites_for_user_in_the_storage()
        {
            OdbFactory.Delete(dbName);
            using (odb = OdbFactory.Open(dbName))
            {
                var updatedUser = UserStorageHelper.GetUser(1);
                updatedUser.ListOfActivitesOnPc= new Activity[0];
                UserStorageHelper.AddActivites(updatedUser);
                var users = UserStorageHelper.GetUsers();
                foreach (var user in users)
                {
                    UserStorage.SaveUserToStore(odb, user);
                }
                var findUser = UserStorage.FindUser(odb, updatedUser);
                UserStorage.SetActivitiesToExistUser(updatedUser,findUser);
                odb.Store(findUser);
                findUser = UserStorage.FindUser(odb, updatedUser);
                Assert.AreEqual(4, findUser.ListOfActivitesOnPc.Count);
                UserStorageHelper.AssertActivites(findUser,1);
            }
        }
    }
}
