using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary.UserLibrary;
using NUnit.Framework;

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
            InitDatabase();

            var usersFromDb = dbTransaction.GetCollectionUsersFromDb();
            var testingUsers = TransformUsers(usersFromDb);
            Assert.AreEqual(users.Count, testingUsers.Count());
            UserAssert(testingUsers);
            UserActivityAssert(testingUsers);
        }

        [Test]
        public void Add_exist_user_to_database()
        {
            InitDatabase();

            var addUser = CreateUser(2);
            addUser.ListOfActivitesOnPc = new List<Activity>() { CreateActivity(5) };
            dbTransaction.AddUser(addUser);

            var usersFromDb = dbTransaction.GetCollectionUsersFromDb();
            var testingUsers = TransformUsers(usersFromDb);
            Assert.AreEqual(users.Count, testingUsers.Count());
            UserAssert(testingUsers);
            UserActivityAssert(testingUsers);
            Assert.AreEqual(6, testingUsers.ToList()[2].ListOfActivitesOnPc.Count);
            Assert.AreEqual("Activity_5", testingUsers.ToList()[2].ListOfActivitesOnPc[5].NameActivity);
        }

        [Test]
        public void Update_user_attention()
        {
            InitDatabase();

            const bool attention = true;

            var usersFromDb = dbTransaction.GetCollectionUsersFromDb();
            var testingUsers = TransformUsers(usersFromDb);

            var singleOrDefault = dbTransaction.GetCollectionUserActivitiesFromDb(0).ToList().SingleOrDefault();
            if (singleOrDefault == null) return;
            long? activityId = singleOrDefault.id_user;
            dbTransaction.UpdateUserActvityAttention(activityId, attention);
            Assert.AreEqual(users.Count, testingUsers.Count());
            UserAssert(testingUsers);
            UserActivityAssert(testingUsers);
            Assert.AreEqual(5, testingUsers.ToList()[0].ListOfActivitesOnPc.Count);
            Assert.AreEqual(true, testingUsers.ToList()[0].ListOfActivitesOnPc[0].Attention);
        }

        [Test]
        public void Add_event_with_observer_attention()
        {
            InitDatabase(false);
            var testingEvent = new TestingEvent()
            {
                NameEvent = "Event01",
                StartEvent = new DateTime(2015, 1, 1)
            };

            var testingObserver = new TestingObserver()
            {
                FirstName = "Lukas",
                LastName = "Rajm"
            };
            var hour = 1;
            foreach (var user in users)
            {
                user.TimeStampDispatch = new DateTime(2015, 1, 1, hour, 0, 0);
                dbTransaction.AddUser(user);
                hour++;
            }

            dbTransaction.AddUser(new User() { UserName = "TestOutSideCollection", PCName = "", TimeStampDispatch = new DateTime(2014, 1, 1) });

            var usersFromDb = dbTransaction.GetUserCollection(new DateTime(2015, 1, 1));
            Assert.AreEqual(4, usersFromDb.Count());

            dbTransaction.AddDateTimeEventWithEventAndObserver(testingEvent, testingObserver);
            var eventFromDb = dbTransaction.GetEvent(testingEvent.NameEvent);
            testingEvent.Id = (int)eventFromDb.id_event;
            var dateTimeEventFromDb = dbTransaction.GetDateTimeEvents(testingEvent.Id);
            dbTransaction.CreateRelationshipBetweenUsersAndDateTimeEvent(dateTimeEventFromDb,usersFromDb);

        }

        private void InitDatabase(bool addUsers = true)
        {
            dbTransaction.DeleteDB();
            if (!addUsers) return;
            foreach (var user in users)
            {
                dbTransaction.AddUser(user);
            }
        }

        private IEnumerable<TestingUser> TransformUsers(IEnumerable<DB_Model.Db_user> usersFromDb)
        {
            return (from dbUser in usersFromDb
                    let activitiesFromDb = dbTransaction.GetCollectionUserActivitiesFromDb(dbUser.id_user)
                    let activities =
                        activitiesFromDb.Select(
                            activity =>
                                new TestingActivity()
                                {
                                    NameActivity = activity.name,
                                    TimeActivity = DateTime.Parse(activity.time_activity),
                                    Attention = false
                                }).ToList()
                    select new TestingUser()
                    {
                        UserName = dbUser.user_name,
                        PCName = dbUser.pc_name,
                        TimeStampDispatch = DateTime.Parse(dbUser.user_timestamp),
                        ListOfActivitesOnPc = activities
                    }).ToList();
        }

        private void UserActivityAssert(IEnumerable<TestingUser> testingUsers)
        {
            foreach (var testingUser in testingUsers)
            {
                var activities = testingUser.ListOfActivitesOnPc;
                for (int i = 0; i < activities.Count; i++)
                {
                    Assert.AreEqual("Activity_" + i, activities[i].NameActivity);
                }
            }
        }

        private static void UserAssert(IEnumerable<TestingUser> testingUsers)
        {
            var dbUsers = testingUsers.ToArray();
            for (int i = 0; i < dbUsers.Count(); i++)
            {
                Assert.AreEqual("User_" + i, dbUsers[i].UserName);
                Assert.AreEqual("PC_" + i, dbUsers[i].PCName);
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

    public class TestingUser : User
    {
        public int Id { get; set; }
        new public IList<TestingActivity> ListOfActivitesOnPc { get; set; }
    }

    public class TestingActivity : IActivity
    {
        public bool Attention { get; set; }
        public string NameActivity { get; set; }
        public DateTime TimeActivity { get; set; }
    }

    public class TestingEvent
    {
        public int Id { get; set; }
        public string NameEvent { get; set; }
        public DateTime StartEvent { get; set; }
        public DateTime EndEvent { get; set; }
    }

    public class TestingObserver
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
