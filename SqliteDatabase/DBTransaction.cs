using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary.UserLibrary;
using SqliteDatabase.DB_Models;

namespace SqliteDatabase
{
    public class DBTransaction
    {
        private const string DateTimeFormate = "dd-MM-yyyy HH:mm:ss";
        private static DBTransaction dbTransaction;

        private DBTransaction()
        { }


        public static DBTransaction ReturnDatabaseInstance()
        {
            return dbTransaction ?? new DBTransaction();
        }

        public void DeleteDB()
        {
            using (var context = new BigBrotherEntities())
            {
                List<Db_user> users = context.Db_user.ToList();
                List<Db_observer> observers = context.Db_observer.ToList();
                List<Db_event> events = context.Db_event.ToList();
                List<Db_date_time_event> dateTimeEvent = context.Db_date_time_event.ToList();
                List<Db_activity> activities = context.Db_activity.ToList();
                List<Db_user_date_time_event> userWork = context.Db_user_date_time_event.ToList();

                context.Db_activity.RemoveRange(activities);
                context.Db_event.RemoveRange(events);
                context.Db_observer.RemoveRange(observers);
                context.Db_date_time_event.RemoveRange(dateTimeEvent);
                context.Db_user.RemoveRange(users);
                context.Db_user_date_time_event.RemoveRange(userWork);
                context.SaveChanges();
            }
        }

        public IEnumerable<Db_user> GetCollectionUsersFromDb()
        {
            using (var context = new BigBrotherEntities())
            {
                return context.Db_user.ToList();
            }
        }

        public IEnumerable<Db_activity> GetCollectionUserActivitiesFromDb(long userId)
        {
            using (var context = new BigBrotherEntities())
            {
                return context.Db_activity.Where(a => a.id_user == userId).ToList();
            }
        }

        #region Operation with user

        public List<Db_user> GetUsersBelongingToDateTimeEvent(int eventId, DateTime startEvenTime)
        {
            using (var context = new BigBrotherEntities())
            {
                Db_date_time_event dateTimeEvent = null;
                foreach (Db_date_time_event dateTime in context.Db_date_time_event)
                {
                    if (dateTime.id_event == eventId && DateTime.Parse(dateTime.start_event) == startEvenTime)
                    {
                        dateTimeEvent = dateTime;
                    }
                }
                return dateTimeEvent != null
                    ? dateTimeEvent.Db_user_date_time_event.Select(ud => ud.Db_user).ToList()
                    : null;
            }
        }

        public void UpdateUserWork(int idUser, int idEvent, string nameWork)
        {
            using (var context = new BigBrotherEntities())
            {
                Db_user user = context.Db_user.Single(u => u.id_user.Equals(idUser));
                Db_date_time_event dateTimeEvent =
                    context.Db_date_time_event.Single(u => u.id_event.Equals(idEvent));
                var userDateTimeEvent = context.Db_user_date_time_event.SingleOrDefault(ud => ud.id_user == user.id_user && dateTimeEvent.id_date_time_event == ud.id_date_time_event);
                userDateTimeEvent.name_work = nameWork;
                context.SaveChanges();
            }
        }

        public void UpdateUserActvityAttention(int? id, bool attention)
        {
            using (var context = new BigBrotherEntities())
            {
                Db_activity activity = context.Db_activity.Single(a => a.id_activity.Equals(id));
                activity.attention =Convert.ToInt32(attention);
                context.SaveChanges();
            }
        }

        public void UpdateUserActvityIgnoreAttention(int? id, bool attention)
        {
            using (var context = new BigBrotherEntities())
            {
                Db_activity activity = context.Db_activity.Single(a => a.id_activity.Equals(id));
                activity.ignore_attention = Convert.ToInt32(attention);
                context.SaveChanges();
            }
        }

        public void AddUser(IUser user)
        {
            using (var context = new BigBrotherEntities())
            {
                Db_user findUser =
                    context.Db_user.SingleOrDefault(
                        u => u.user_name.Equals(user.UserName) && u.pc_name.Equals(user.PCName));
                if (findUser == null)
                    InsertUser(context, user);
                else
                {
                    UpdateUser(findUser, user);
                }
                context.SaveChanges();
            }
        }

        private void UpdateUser(Db_user findUseruser, IUser user)
        {
            AddActivity(findUseruser, user);
            findUseruser.user_timestamp = GetUserTimestamp(user);
        }

        private static string GetUserTimestamp(IUser user)
        {
            return user.TimeStampDispatch.ToString(DateTimeFormate);
        }

        private static void AddActivity(Db_user findUseruser, IUser user)
        {
            foreach (Db_activity activity in user.ListOfActivitesOnPc.Select(item => new Db_activity
            {
                Db_user = findUseruser,
                name = item.NameActivity,
                time_activity = item.TimeActivity.ToString(DateTimeFormate)
                
            }))
            {
                findUseruser.Db_activity.Add(activity);
            }
        }

        private static void InsertUser(BigBrotherEntities context, IUser user)
        {
            var dbUser = new Db_user
            {
                pc_name = user.PCName,
                user_name = user.UserName,
                user_timestamp = GetUserTimestamp(user)
            };

            AddActivity(dbUser, user);
            context.Db_user.Add(dbUser);
        }

        public void CreateRelationshipBetweenUsersAndDateTimeEvent(Db_date_time_event dateTimeEvent,
            IEnumerable<Db_user> users)
        {
            using (var context = new BigBrotherEntities())
            {
                List<Db_user> usersFromDb = context.Db_user.ToList();
                IEnumerable<Db_user> dbUsers = usersFromDb.Where(
                    userDb => users.Any(user => user.user_name == userDb.user_name && user.pc_name == userDb.pc_name));

                Db_date_time_event dateTimeEventFromDb =
                    context.Db_date_time_event.Single(d => d.id_date_time_event == dateTimeEvent.id_date_time_event);

                foreach (Db_user user in dbUsers)
                {
                    context.Db_user_date_time_event.Add(new Db_user_date_time_event
                    {
                        Db_user = user,
                        Db_date_time_event = dateTimeEventFromDb
                    });
                }

                context.SaveChanges();
            }
        }

        #endregion

        #region Operation with events

        public void AddEventWithObserver(string nameEvent, string fistNameObserver, string lastNameObserver, DateTime startEvent)
        {
            using (var context = new BigBrotherEntities())
            {
                var @event = new Db_event { event_name = nameEvent };
                var observer = new Db_observer
                {
                    first_name = fistNameObserver,
                    last_name = lastNameObserver
                };
                var dateTimeEvent = new Db_date_time_event
                {
                    Db_event = @event,
                    Db_observer = observer,
                    start_event = startEvent.ToString(DateTimeFormate)
                };

                context.Db_date_time_event.Add(dateTimeEvent);
                context.SaveChanges();
            }
        }

        public Db_date_time_event GetDateTimeEvent(int eventId)
        {
            using (var context = new BigBrotherEntities())
            {
                return context.Db_date_time_event.SingleOrDefault(d => d.id_event == eventId);
            }
        }

        public Db_observer GetObserver(int observerId)
        {
            using (var context = new BigBrotherEntities())
            {
                return context.Db_observer.SingleOrDefault(o => o.id_observer == observerId);
            }
        }

        public Db_event GetEvent(string nameEvent)
        {
            using (var context = new BigBrotherEntities())
            {
                return context.Db_event.SingleOrDefault(e => e.event_name == nameEvent);
            }
        }

        public IEnumerable<Db_user> GetUserCollection(DateTime starTimeEvent)
        {
            var userList = new List<Db_user>();
            using (var context = new BigBrotherEntities())
            {
                // ReSharper disable once LoopCanBeConvertedToQuery dateTime parse doesn't work in linq
                foreach (Db_user dbUser in context.Db_user)
                {
                    if (DateTime.Parse(dbUser.user_timestamp) >= starTimeEvent)
                    {
                        userList.Add(dbUser);
                    }
                }
                return userList;
            }
        }


        public void UpdateDateTimeEventFinish(int id, DateTime finish)
        {
            using (var context = new BigBrotherEntities())
            {
                context.Db_date_time_event.Single(d => d.id_date_time_event.Equals(id)).end_event =
                    finish.ToString(DateTimeFormate);
                context.SaveChanges();
            }
        }
        #endregion
    }
}