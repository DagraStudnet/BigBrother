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
        private List<MonitoringUser> users;
        public  List<Attention> Attentions { get; set; } 
        
        public ReadWriteDB(Event @event, List<Attention> attentions)
        {
            EventInstance = @event;
            dbTransaction = DBTransaction.ReturnDatabaseInstance();
            Attentions = attentions;
        }

        public Event EventInstance { get; set; }

        public void SaveEventWithObserverToDb()
        {
            dbTransaction.AddEventWithObserver(EventInstance.NameEvent, EventInstance.ObserverEvent.FirstName,
                EventInstance.ObserverEvent.LastName, EventInstance.StarTimeEvent);
            GetEventId();
        }

        public void SaveEventFinishToDb()
        {
            var dateTimeEvent = dbTransaction.GetDateTimeEvent(EventInstance.Id, EventInstance.StarTimeEvent);
            dbTransaction.UpdateDateTimeEventFinish((int)dateTimeEvent.id_date_time_event, EventInstance.EndTimeEvent);
        }
        
        public void SaveNameWorkUserToDb(object sender, EventArgs e)
        {
            var user = (sender as MonitoringUser);
            dbTransaction.UpdateUserWork(user.Id, EventInstance.Id,EventInstance.StarTimeEvent, user.NameWork);
        }

        public void SaveRelationshipBetweenEventAndUsers()
        {
            Db_date_time_event dateTimeEventFromDB = dbTransaction.GetDateTimeEvent(EventInstance.Id,EventInstance.StarTimeEvent);
            DateTime startTimeEvent = DateTime.Parse(dateTimeEventFromDB.start_event);
            List<Db_user> usersFromDB =
                dbTransaction.GetUserCollection(startTimeEvent).ToList();
            dbTransaction.CreateRelationshipBetweenUsersAndDateTimeEvent(dateTimeEventFromDB, usersFromDB);
        }

        private void GetEventId()
        {
            EventInstance.Id = (int)dbTransaction.GetEventByName(EventInstance.NameEvent).id_event;
        }

        public IEnumerable<Db_user> GetUsersWithOutEventFromDb()
        {
            List<Db_user> usersDateTimeEvent = dbTransaction.GetUsersBelongingToDateTimeEvent(EventInstance.Id,
                EventInstance.StarTimeEvent);
            var users = dbTransaction.GetUserCollection(EventInstance.StarTimeEvent);
            return usersDateTimeEvent.Count > 0 ? users.Where(user => usersDateTimeEvent.Any(userDateTimeEvent => userDateTimeEvent.id_user != user.id_user)).ToList() : users;
        }

        public IEnumerable<MonitoringUser> GetUsersWithEventFromDb()
        {
            var dbUsers = GetUsersBelongingToDateTimeEvent().ToList();
            var usersListWithOutEvent = GetUsersWithOutEventFromDb().ToList();
            if (dbUsers.Count() <= usersListWithOutEvent.Count())
            {
                var anotherUsers = new List<Db_user>();

                foreach (var user in usersListWithOutEvent)
                {
                    var userEquals = false;
                    foreach (var dbUser in dbUsers)
                    {
                        if (user.id_user == dbUser.id_user)
                            userEquals = true;
                    }
                    if (userEquals)
                        continue;
                    anotherUsers.Add(user);
                }
                var dateTimeEventFromDb = dbTransaction.GetDateTimeEvent(EventInstance.Id, EventInstance.StarTimeEvent);
                dbTransaction.CreateRelationshipBetweenUsersAndDateTimeEvent(dateTimeEventFromDb, anotherUsers);
            }
            dbUsers = GetUsersBelongingToDateTimeEvent().ToList();
            users = GetTransformatoinUsersList(dbUsers).ToList();
            SetUsersNameWork(users);
            SetAttentionUser(users);
            SaveUserWorkNameEvent(users);
            return users;
        }

        private void SetUsersNameWork(IEnumerable<MonitoringUser> monitoringUsers)
        {
            foreach (var monitoringUser in monitoringUsers)
            {
                monitoringUser.NameWork = dbTransaction.GetUserNameWork(monitoringUser.Id,EventInstance.Id,EventInstance.StarTimeEvent);
            }
            
        }

        private void SetAttentionUser(IEnumerable<MonitoringUser> usersList)
        {
            foreach (var monitoringUser in usersList)
            {
                var activities = dbTransaction.GetCollectionUserActivitiesFromDb(monitoringUser.Id, EventInstance.StarTimeEvent);
                foreach (var activity in activities)
                {
                    if (ExisUsertAttention(activity))
                        monitoringUser.Attention = true;
                }

            }
        }

        private void SaveUserWorkNameEvent(IEnumerable<MonitoringUser> usersList)
        {
            foreach (var monitoringUser in usersList)
            {
                monitoringUser.PropertyChanged += SaveNameWorkUserToDb;
            }
        }

        private void RefreshUserAttention(object sender, PropertyChangedEventArgs e)
        {
            var activity = (sender as MonitoringActivity);
            var userId = dbTransaction.GetUserIdFromActivityDb(activity.Id);
            var activities = dbTransaction.GetCollectionUserActivitiesFromDb(userId, EventInstance.StarTimeEvent);
            var findAttention = activities.Any(ExisUsertAttention);
            //dbTransaction.UpdateUserAttention(userId, Convert.ToInt32(findAttention));
            var user = users.Find(u => u.Id == userId);
            user.Attention = findAttention;
        }

        private bool ExisUsertAttention(Db_activity dbActivity)
        {
            //return Convert.ToBoolean(dbActivity.attention) && !Convert.ToBoolean(dbActivity.ignore_attention);
            return Attentions.Any(a => dbActivity.name.Contains(a.Name)) && !Convert.ToBoolean(dbActivity.ignore_attention); 
        }

        private IEnumerable<Db_user> GetUsersBelongingToDateTimeEvent()
        {
            return dbTransaction.GetUsersBelongingToDateTimeEvent(EventInstance.Id, EventInstance.StarTimeEvent);
        }

        private IEnumerable<MonitoringUser> GetTransformatoinUsersList(IEnumerable<Db_user> dbUsers)
        {
            users = dbUsers.Select(dbUser =>
            {
                var user = TransformationValuesFromDatabase.TransformUserFromDB(dbUser);
                if (!user.Connection)
                {
                    dbTransaction.SetUserDisconnectActivity(user.Id, "Disconnect");
                }
                return user;
            }).ToList();
            return users;
        }

        public IEnumerable<MonitoringActivity> GetUserActivities(int userId)
        {
            var monitoringActivities =
                dbTransaction.GetCollectionUserActivitiesFromDb(userId,EventInstance.StarTimeEvent)
                    .Select(TransformationValuesFromDatabase.TransformActivityFromDB)
                    .ToList();
            ExistUserActivityAttention(monitoringActivities);
            SetupEventsForPropertyChanged(monitoringActivities);
            return monitoringActivities;
        }

        private void ExistUserActivityAttention(IEnumerable<MonitoringActivity> monitoringActivities)
        {
            foreach (var activity in monitoringActivities)
            {
                activity.Attention = Attentions.Any(a => activity.NameActivity.Contains(a.Name));    
            }
            
        }

        private void SetupEventsForPropertyChanged(IEnumerable<MonitoringActivity> monitoringActivities)
        {
            foreach (var monitoringActivity in monitoringActivities)
            {
                monitoringActivity.PropertyChanged += SaveIngnoreAttentionToDb;
                monitoringActivity.PropertyChanged += RefreshUserAttention;
            }
        }

        private void SaveIngnoreAttentionToDb(object sender, PropertyChangedEventArgs e)
        {
            var activity = (sender as MonitoringActivity);
            dbTransaction.UpdateUserActvityIgnoreAttention(activity.Id, activity.IgnoreAttention);
        }


    }
}