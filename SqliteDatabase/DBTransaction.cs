using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary.UserLibrary;

namespace SqliteDatabase
{
    public class DBTransaction
    {
        private const string DateTimeFormate = "dd-MM-yyyy HH:MM:ss";
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
                var users = context.Db_user.ToList();
                var events = context.Db_event.ToList();
                var dateTimeEvent = context.Db_date_time_event.ToList();
                var activities = context.Db_activity.ToList();
                var userTimestamp = context.Db_user_timestamp.ToList();
                var userWork = context.Db_user_work.ToList();

                context.Db_activity.RemoveRange(activities);
                context.Db_event.RemoveRange(events);
                context.Db_date_time_event.RemoveRange(dateTimeEvent);
                context.Db_user.RemoveRange(users);
                context.Db_user_timestamp.RemoveRange(userTimestamp);
                context.Db_user_work.RemoveRange(userWork);
                context.SaveChanges();
            }
        }

        #region Operation with user

        public void UpdateUserWork(int idUser, int idEvent, string nameWork)
        {
            using (var context = new BigBrotherEntities())
            {
                var user = context.Db_user.Single(u => u.id_user.Equals(idUser));
                var @event = context.Db_event.Single(u => u.id_event.Equals(idEvent));
                var userWork = new Db_user_work()
                {
                    Db_event = @event,
                    Db_user = user,
                    name_work = nameWork
                };
                context.Db_user_work.Add(userWork);
                context.SaveChanges();
            }
        }

        public void UpdateUserActvityAttention(int id, bool attention)
        {
            using (var context = new BigBrotherEntities())
            {
                var activity = context.Db_activity.Single(a => a.id_activity.Equals(id));
                activity.attention = attention;
                context.SaveChanges();
            }
        }

        public void AddUser(IUser user)
        {
            using (var context = new BigBrotherEntities())
            {
                var findUser =
                    context.Db_user.SingleOrDefault(u => u.user_name.Equals(user.UserName) && u.pc_name.Equals(user.PCName));
                if (findUser == null)
                    InsertUser(context, user);
                else
                {
                    UpdateUser(context, findUser, user);
                }
                context.SaveChanges();
            }
        }

        private void UpdateUser(BigBrotherEntities context, Db_user findUseruser, IUser user)
        {
            AddActivity(findUseruser, user);
            AddUserTimestamp(context, user, findUseruser);
        }

        private static void AddActivity(Db_user findUseruser, IUser user)
        {
            foreach (var activity in user.ListOfActivitesOnPc.Select(item => new Db_activity()
            {
                Db_user = findUseruser,
                name = item.NameActivity,
                time_activity = item.TimeActivity.ToString(DateTimeFormate),
                attention = false
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
                user_name = user.UserName
            };

            AddUserTimestamp(context, user, dbUser);
            AddActivity(dbUser, user);
            context.Db_user.Add(dbUser);
        }

        private static void AddUserTimestamp(BigBrotherEntities context, IUser user, Db_user dbUser)
        {
            var userTimestamp = new Db_user_timestamp()
            {
                user_timestamp = user.TimeStampDispatch.ToString(DateTimeFormate),
                Db_user = dbUser
            };
            context.Db_user_timestamp.Add(userTimestamp);
        }
        #endregion

        #region Operation with events

        public void AddDateTimeEventWithEventAndObserver()
        {
            using (var context = new BigBrotherEntities())
            {
                var @event = new Db_event() { event_name = "" };
                var observer = new Db_observer() { first_name = "", last_name = "" };
                var dateTimeEvent = new Db_date_time_event() { Db_event = @event, Db_observer = observer, start_event = "", end_event = "" };
                context.Db_date_time_event.Add(dateTimeEvent);
                context.SaveChanges();
            }
        }

        public int GetDateTimeEventId()
        {
            using (var context = new BigBrotherEntities())
            {
                var id = context.Db_date_time_event.SingleOrDefault(e => e.Db_event.Equals("")).id_date_time_event;
                return (int)id;
            }
        }

        public void UpdateDateTimeEventFinish(int id, DateTime finish)
        {
            using (var context = new BigBrotherEntities())
            {
                context.Db_date_time_event.Single(d => d.id_date_time_event.Equals(id)).end_event = finish.ToString(DateTimeFormate);
                context.SaveChanges();
            }
        }
        #endregion

        public IEnumerable<Db_user> GetCollectionUsersFromDb()
        {
            using (var context = new BigBrotherEntities())
            {
                return context.Db_user.ToList();
            }
        }
    }
}