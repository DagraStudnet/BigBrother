using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SqliteDatabase;
using SqliteDatabase.DB_Models;

namespace HostingBigBrother.Model
{
    public class ReadWriteDB
    {
        private readonly DBTransaction dbTransaction;
        
        //public ReadWriteDB(MonitoringUsersCollection monitoringUsers, Event @event)
        public ReadWriteDB(Event @event)
        {
            //MonitoringUsers = monitoringUsers;
            EventInstance = @event;
            dbTransaction = DBTransaction.ReturnDatabaseInstance();
        }

        //public MonitoringUsersCollection MonitoringUsers { get; set; }
        public Event EventInstance { get; set; }

        public void SaveEventWithObserverToDb()
        {
            dbTransaction.AddEventWithObserver(EventInstance.NameEvent, EventInstance.ObserverEvent.FirstName,
                EventInstance.ObserverEvent.LastName, EventInstance.StarTimeEvent);
        }

        public void SaveEventFinishToDb()
        {
            dbTransaction.UpdateDateTimeEventFinish(EventInstance.Id, EventInstance.EndTimeEvent);
        }

        public void SaveNameWorkUserToDb(int userId, string nameWork)
        {
            dbTransaction.UpdateUserWork(userId, EventInstance.Id, nameWork);
        }

        public void SaveNameWorkUserToDb(object sender, EventArgs e)
        {
            var user = (sender as MonitoringUser);
            dbTransaction.UpdateUserWork(user.Id, EventInstance.Id, user.NameWork);
        }

        public void SaveUserActivityAttentionToDb(int userId, bool attention)
        {
            dbTransaction.UpdateUserActvityAttention(userId, attention);
        }

        public void SaveRelationshipBetweenEventAndUsers()
        {
            Db_date_time_event dateTimeEventFromDB = dbTransaction.GetDateTimeEvent(EventInstance.Id);
            DateTime startTimeEvent = DateTime.Parse(dateTimeEventFromDB.start_event);
            List<Db_user> usersFromDB =
                dbTransaction.GetUserCollection(startTimeEvent).ToList();
            dbTransaction.CreateRelationshipBetweenUsersAndDateTimeEvent(dateTimeEventFromDB, usersFromDB);
        }

        public void GetEventId()
        {
            EventInstance.Id = (int) dbTransaction.GetEvent(EventInstance.NameEvent).id_event;
        }

        public IEnumerable<Db_user> GetUsersWithOutEventFromDb()
        {
            List<Db_user> usersDateTimeEvent = dbTransaction.GetUsersBelongingToDateTimeEvent(EventInstance.Id,
                EventInstance.StarTimeEvent);
            return
                dbTransaction.GetUserCollection(EventInstance.EndTimeEvent)
                    .Where(
                        user => usersDateTimeEvent.Any(userDateTimeEvent => userDateTimeEvent.id_user != user.id_user));
        }

        public IEnumerable<MonitoringUser> GetUsersWithEventFromDb()
        {
            IEnumerable<Db_user> dbUsers = GetUsersBelongingToDateTimeEvent();
            IEnumerable<MonitoringUser> usersList = GetTransformatoinUsersList(dbUsers);
            SaveUserWorkNameEvent(usersList);
            IEnumerable<Db_user> usersListWithOutEvent = GetUsersWithOutEventFromDb();
            if (usersList.Count() >= usersListWithOutEvent.Count())
                return usersList;
            var anotherUsers = new List<Db_user>();

            foreach (var anotherUser in usersListWithOutEvent)
            {
                var userEquals = false;
                foreach (var dbUser in dbUsers)
                {
                    if (anotherUser.id_user == dbUser.id_user)
                        userEquals = true;
                }
                if (userEquals)
                    continue;
                anotherUsers.Add(anotherUser);
            }
            Db_date_time_event dateTimeEventFromDB = dbTransaction.GetDateTimeEvent(EventInstance.Id);
            dbTransaction.CreateRelationshipBetweenUsersAndDateTimeEvent(dateTimeEventFromDB, anotherUsers);
            dbUsers = GetUsersBelongingToDateTimeEvent();
            usersList = GetTransformatoinUsersList(dbUsers);
            SaveUserWorkNameEvent(usersList);
            return usersList;
        }

        private void SaveUserWorkNameEvent(IEnumerable<MonitoringUser> usersList)
        {
            foreach (var monitoringUser in usersList)
            {
                monitoringUser.PropertyChanged += SaveNameWorkUserToDb;
            }
        }

        private IEnumerable<Db_user> GetUsersBelongingToDateTimeEvent()
        {
            return dbTransaction.GetUsersBelongingToDateTimeEvent(EventInstance.Id, EventInstance.StarTimeEvent);
        }

        private IEnumerable<MonitoringUser> GetTransformatoinUsersList(IEnumerable<Db_user> dbUsers)
        {
            return dbUsers.Select(dbUser =>
            {
                MonitoringUser user = TransformationValuesFromDatabase.TransformUserFromDB(dbUser);
                return user;
            }).ToList();
        }

        public IEnumerable<MonitoringActivity> GetUserActivities(int userId)
        {
            var monitoringActivities =
                dbTransaction.GetCollectionUserActivitiesFromDb(userId)
                    .Select(TransformationValuesFromDatabase.TransformActivityFromDB)
                    .ToList();
            SaveIngnoreAttention(monitoringActivities);
            return monitoringActivities;
        }

        private void SaveIngnoreAttention(IEnumerable<MonitoringActivity> monitoringActivities)
        {
            foreach (var monitoringActivity in monitoringActivities)
            {
                monitoringActivity.PropertyChanged += SaveIngnoreAttentionToDb;
            }
        }

        private void SaveIngnoreAttentionToDb(object sender, PropertyChangedEventArgs e)
        {
            var activity = (sender as MonitoringActivity);
            dbTransaction.UpdateUserActvityIgnoreAttention(activity.Id, activity.IgnoreAttention);
        }
    }
}