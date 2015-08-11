using System.Collections.Generic;
using System.Linq;
using ClassLibrary.UserLibrary;
using NUnit.Framework;
using SqliteDatabase;

namespace ClientTests
{
    [TestFixture]
    public class TestDBTransaction
    {
        private CloneDBTransaction dbTransaction;
        private List<IUser> users;

        [SetUp]
        public void Setup()
        {
            dbTransaction = CloneDBTransaction.ReturnDatabaseInstance();
            dbTransaction.DeleteDB();
            users = CreateUsersCollection();
        }

        [Test]
        public void Load_testing_users()
        {
            Assert.AreEqual(4, users.Count);
        }

        [Test]
        public void Add_users_to_database()
        {
            foreach (var user in users)
            {
                dbTransaction.AddUser(user);
            }
            var usersFromDb = dbTransaction.GetCollectionUsersFromDb();
            Assert.AreEqual(users.Count, usersFromDb.ToList().Count);
            UserAssert(usersFromDb);
            UserActivityAssert(usersFromDb);
        }

        private void UserActivityAssert(IEnumerable<DB_Model.Db_user> usersFromDb)
        {
            foreach (var dbUser in usersFromDb)
            {
                var activities = dbUser.Db_activity.ToArray();
                for (int i = 0; i < activities.Length; i++)
                {
                    Assert.AreEqual("Activity_" + i, activities[i].name);
                }
            }
        }

        private static void UserAssert(IEnumerable<DB_Model.Db_user> usersFromDb)
        {
            var dbUsers = usersFromDb.ToArray();
            for (int i = 0; i < dbUsers.Count(); i++)
            {
                Assert.AreEqual("User_" + i, dbUsers[i].user_name);
                Assert.AreEqual("PC_" + i, dbUsers[i].pc_name);
            }
        }


        private List<IUser> CreateUsersCollection()
        {
            var users = new List<IUser>();
            for (var i = 0; i < 4; i++)
            {
                var user = CreateUser(i);
                users.Add(user);
                for (var j = 0; j < 5; j++)
                {
                    var activity = CreateActivity(j);
                    user.ListOfActivitesOnPc.Add(activity);
                }
            }
            return users;
        }

        private User CreateUser(int id)
        {
            return new User()
            {
                PCName = "PC_" + id,
                UserName = "User_" + id,
                ListOfActivitesOnPc = new List<Activity>()
            };
        }

        private Activity CreateActivity(int id)
        {
            return new Activity()
            {
                NameActivity = "Activity_" + id
            };
        }
    }
}
