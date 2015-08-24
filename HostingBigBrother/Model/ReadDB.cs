using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqliteDatabase;
using SqliteDatabase.DB_Models;

namespace HostingBigBrother.Model
{
    public class ReadDB
    {
        private readonly List<Attention> _attentions;
        private readonly DBTransaction dbTransaction;
        private List<MonitoringUser> monitoringUsers;
        private DateTime starTimeEventValue;
        private DateTime endTimeEventValue;

        public ReadDB(List<Attention> attentions)
        {
            _attentions = attentions;
            dbTransaction = DBTransaction.ReturnDatabaseInstance();
        }

        public IEnumerable<Event> GetAllEvents()
        {
            var allEventFromDb = dbTransaction.GetAllEvents();
            return allEventFromDb.Select(e => new Event() { Id = (int)e.id_event, NameEvent = e.event_name });
        }

        public IEnumerable<MonitoringUser> GetAllUsers()
        {
            var alluserFromDb = dbTransaction.GetAllUsers();
            return alluserFromDb.Select(u => new MonitoringUser() { Id = (int)u.id_user, UserName = u.user_name });
        }

        public IEnumerable<Observer> GetAllObservers()
        {
            var allObserversFromDb = dbTransaction.GetAllObservers();
            return allObserversFromDb.Select(o => new Observer() { Id = (int)o.id_observer, FirstName = o.first_name, LastName = o.last_name });
        }

        public IEnumerable<Event> GetFiltredEvent(int selectedEventId, int selectedObserverId, int selectedUserId, DateTime? selectedStartEvent, DateTime? selectedEndEvent)
        {
            var dbDateTimeEvents = dbTransaction.GetAllDateTimeEven();

            if (selectedEventId != 0)
                dbDateTimeEvents = dbDateTimeEvents.Where(dte => dte.id_event == selectedEventId);
            if (selectedObserverId != 0)
                dbDateTimeEvents = dbDateTimeEvents.Where(dte => dte.id_observer == selectedObserverId);
            if (selectedUserId != 0)
            {
                var dbUserDateTimeEvents = dbTransaction.GetUserDateTimeEvent(selectedUserId);
                dbDateTimeEvents = dbDateTimeEvents.Where(dte => dbUserDateTimeEvents.Any(udte => udte.id_date_time_event == dte.id_date_time_event));
            }

            if (selectedStartEvent != null)
            {
                var dateTimeEventsHelper = new List<Db_date_time_event>();
                foreach (var dbDateTimeEvent in dbDateTimeEvents)
                {
                    if (!(DateTime.Parse(dbDateTimeEvent.start_event) >= selectedStartEvent)) continue;
                    if ((DateTime.Parse(dbDateTimeEvent.end_event) <= selectedEndEvent.Value.AddHours(23).AddMinutes(59)))
                    {
                        dateTimeEventsHelper.Add(dbDateTimeEvent);
                    }
                }
                dbDateTimeEvents = dateTimeEventsHelper;
            }
            var eventCollection = new List<Event>();
            foreach (var dbDateTimeEvent in dbDateTimeEvents)
            {
                var dbEvent = dbTransaction.GetEventById(dbDateTimeEvent.id_event);
                var dbObserver = dbTransaction.GetObserverById((int)dbDateTimeEvent.id_observer);
                eventCollection.Add(new Event()
                {
                    Id = (int)dbEvent.id_event,
                    NameEvent = dbEvent.event_name,
                    StarTimeEvent = DateTime.Parse(dbDateTimeEvent.start_event),
                    EndTimeEvent = DateTime.Parse(dbDateTimeEvent.end_event),
                    ObserverEvent = new Observer()
                    {
                        Id = (int)dbObserver.id_observer,
                        FirstName = dbObserver.first_name,
                        LastName = dbObserver.last_name
                    }
                });
            }
            return eventCollection;
        }

        public List<MonitoringActivity> GetUserActivities(int userId, DateTime starTimeEvent, DateTime endTimeEvent)
        {
            var monitoringActivities =
                dbTransaction.GetCollectionUserActivitiesFromDb(userId, starTimeEvent, endTimeEvent)
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
                activity.Attention = _attentions.Any(a => activity.NameActivity.Contains(a.Name));
            }
        }

        public List<MonitoringUser> GetEventUsers(int eventId, DateTime starTimeEvent, DateTime endTimeEvent)
        {
            starTimeEventValue = starTimeEvent;
            endTimeEventValue = endTimeEvent;
            monitoringUsers = dbTransaction.GetUsersBelongingToDateTimeEvent(eventId, starTimeEvent).Select(TransformationValuesFromDatabase.TransformUserFromDB).ToList();
            foreach (var monitoringUser in monitoringUsers)
            {
                monitoringUser.NameWork = dbTransaction.GetUserNameWork(monitoringUser.Id, eventId, starTimeEvent);
                monitoringUser.Attention = GetUserActivities(monitoringUser.Id, starTimeEvent, endTimeEvent).Any(a => _attentions.Any(at => a.NameActivity.Contains(at.Name) && !a.IgnoreAttention));
            }
            return monitoringUsers;
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

        private void RefreshUserAttention(object sender, PropertyChangedEventArgs e)
        {
            var activity = (sender as MonitoringActivity);
            var userId = dbTransaction.GetUserIdFromActivityDb(activity.Id);
            var activities = dbTransaction.GetCollectionUserActivitiesFromDb(userId, starTimeEventValue,endTimeEventValue);
            var findAttention = activities.Any(ExisUsertAttention);
            var user = monitoringUsers.Find(u => u.Id == userId);
            user.Attention = findAttention;
        }
        private bool ExisUsertAttention(Db_activity dbActivity)
        {
            return _attentions.Any(a => dbActivity.name.Contains(a.Name)) && !Convert.ToBoolean(dbActivity.ignore_attention);
        }
    }
}
