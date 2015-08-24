using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HostingBigBrother.Annotations;
using HostingBigBrother.Model;

namespace HostingBigBrother.ViewModel
{
    public class HistoricalEventDataViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly List<Attention> _attentions;
        private readonly ReadDB readDb;

        private ObservableCollection<Event> _eventCombobox;


        private ObservableCollection<Event> _filtredEventDataGird;
        private ObservableCollection<MonitoringUser> _filtredUserDataGird;
        private ObservableCollection<Observer> _observerCombox;
        private DateTime? _selectedEndEvent;

        private int _selectedEventId;
        private int _selectedObserverId;
        private DateTime? _selectedStartEvent;
        private int _selectedUserId;
        private ObservableCollection<MonitoringActivity> _userActivitiesDataGird;
        private ObservableCollection<MonitoringUser> _userCombobox;
        private MonitoringUser selectedUserValue;
        private Event selectedEventValue;

        public HistoricalEventDataViewModel(List<Attention> attentions)
        {
            readDb = new ReadDB(attentions);
            EventCombobox = new ObservableCollection<Event>(readDb.GetAllEvents());
            ObserverCombox = new ObservableCollection<Observer>(readDb.GetAllObservers());
            UserCombobox = new ObservableCollection<MonitoringUser>(readDb.GetAllUsers());
            FiltredEventDataGird = new ObservableCollection<Event>(readDb.GetFiltredEvent(0, 0, 0, null, null));
        }

        public ObservableCollection<Event> FiltredEventDataGird
        {
            get { return _filtredEventDataGird; }
            set
            {
                if (Equals(value, _filtredEventDataGird)) return;
                _filtredEventDataGird = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MonitoringUser> FiltredUserDataGird
        {
            get { return _filtredUserDataGird; }
            set
            {
                if (Equals(value, _filtredUserDataGird)) return;
                _filtredUserDataGird = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MonitoringActivity> UserActivitiesDataGird
        {
            get { return _userActivitiesDataGird; }
            set
            {
                if (Equals(value, _userActivitiesDataGird)) return;
                _userActivitiesDataGird = value;
                OnPropertyChanged();
            }
        }


        public int SelectedEventId
        {
            get { return _selectedEventId; }
            set
            {
                if (value == _selectedEventId) return;
                _selectedEventId = value;
                OnPropertyChanged();
            }
        }

        public int SelectedObserverId
        {
            get { return _selectedObserverId; }
            set
            {
                if (value == _selectedObserverId) return;
                _selectedObserverId = value;
                OnPropertyChanged();
            }
        }

        public int SelectedUserId
        {
            get { return _selectedUserId; }
            set
            {
                if (value == _selectedUserId) return;
                _selectedUserId = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Event> EventCombobox
        {
            get { return _eventCombobox; }
            set
            {
                if (Equals(value, _eventCombobox)) return;
                _eventCombobox = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Observer> ObserverCombox
        {
            get { return _observerCombox; }
            set
            {
                if (Equals(value, _observerCombox)) return;
                _observerCombox = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MonitoringUser> UserCombobox
        {
            get { return _userCombobox; }
            set
            {
                if (Equals(value, _userCombobox)) return;
                _userCombobox = value;
                OnPropertyChanged();
            }
        }

        public DateTime? SelectedStartEvent
        {
            get { return _selectedStartEvent; }
            set
            {
                if (value.Equals(_selectedStartEvent)) return;
                _selectedStartEvent = value;
                OnPropertyChanged();
                OnPropertyChanged("SelectedEndEvent");
            }
        }

        public DateTime? SelectedEndEvent
        {
            get { return _selectedEndEvent; }
            set
            {
                if (value.Equals(_selectedEndEvent)) return;
                _selectedEndEvent = value;
                OnPropertyChanged();
                OnPropertyChanged("SelectedStartEvent");
            }
        }


        public Event SelectedEvent
        {
            get { return selectedEventValue; }
            set
            {
                if (value.Equals(selectedEventValue)) return;
                selectedEventValue = value;
                selectedUserValue = null;
                OnPropertyChanged("Users");
                OnPropertyChanged("Activities");
            }
        }

        public MonitoringUser SelectedUser
        {
            get { return selectedUserValue; }
            set
            {
                if (value.Equals(selectedUserValue)) return;
                selectedUserValue = value;
                OnPropertyChanged("Activities");
            }
        }

        public ObservableCollection<MonitoringActivity> Activities
        {
            get { return SelectedUser == null ? null : LoadActitvities(SelectedUser); }
        }

        private ObservableCollection<MonitoringUser> LoadUsers(Event selectedEvent)
        {
            var usersCollection = readDb.GetEventUsers(selectedEvent.Id, SelectedEvent.StarTimeEvent,
                SelectedEvent.EndTimeEvent);
            return usersCollection == null ? null : new ObservableCollection<MonitoringUser>(usersCollection);
        }

        public ObservableCollection<MonitoringUser> Users
        {
            get { return SelectedEvent == null ? null : LoadUsers(SelectedEvent); }
        }

        private ObservableCollection<MonitoringActivity> LoadActitvities(MonitoringUser user)
        {
            return new ObservableCollection<MonitoringActivity>(readDb.GetUserActivities(user.Id, SelectedEvent.StarTimeEvent, SelectedEvent.EndTimeEvent));
        }

        public string this[string columnName]
        {
            get
            {
                if (SelectedStartEvent == null && SelectedEndEvent == null)
                    return null;
                if (columnName == "SelectedStartEvent" && SelectedStartEvent > SelectedEndEvent)
                    return "From datepicker mustn't be greater To datepicker";
                if (columnName == "SelectedEndEvent" && SelectedStartEvent > SelectedEndEvent)
                    return "To datepicker mustn't be lower Form datepicker";
                if (columnName == "SelectedStartEvent" && SelectedStartEvent != null && SelectedEndEvent == null)
                    return "To datepicker must be fill";
                if (columnName == "SelectedStartEvent" && SelectedStartEvent == null && SelectedEndEvent != null)
                    return "From datepicker must be fill";
                if (columnName == "SelectedEndEvent" && SelectedStartEvent != null && SelectedEndEvent == null)
                    return "To datepicker must be fill";
                if (columnName == "SelectedEndEvent" && SelectedStartEvent == null && SelectedEndEvent != null)
                    return "From datepicker must be fill";

                return null;
            }
        }

        public string Error { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FilterData()
        {
            FiltredEventDataGird =
                new ObservableCollection<Event>(readDb.GetFiltredEvent(SelectedEventId, SelectedObserverId,
                    SelectedUserId, SelectedStartEvent, SelectedEndEvent));
            SelectedEvent = new Event();
            SelectedUser = new MonitoringUser();
        }
    }
}