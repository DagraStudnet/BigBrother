using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Windows.Threading;
using HostingBigBrother.Model;
using WcfServiceLibrary;

namespace HostingBigBrother.ViewModel
{
    public class ViewModelMain : BindableBase, INotifyPropertyChanged
    {
        private ReadWriteDB readWriteDb;

        private ServiceHost serviceHost;

        private ObservableCollection<MonitoringUser> users;
        private ObservableCollection<MonitoringActivity> activities;
        private MonitoringUser selectedUserValue;
        private List<Attention> _attentions;

        public List<Attention> Attentions
        {
            get { return _attentions; }
            set
            {
                SetProperty(ref _attentions, value);
                if (value.Count > 0 && readWriteDb != null) readWriteDb.Attentions = _attentions;
                RaiseNotification("Attentions");
            }
        }

        public Event EventView { get; set; }

        public ViewModelMain()
        {

            Attentions = new List<Attention>();
            StartHosting();
            Attentions.Add(new Attention { Name = "Visual" });
            Attentions.Add(new Attention { Name = "Not" });
            Attentions.Add(new Attention { Name = "USB" });
            Attentions.Add(new Attention { Name = "Lite" });
        }


        public ObservableCollection<MonitoringUser> Users
        {
            get { return users; }
            set
            {
                SetProperty(ref users, value);
                RaiseNotification("Users");
            }
        }

        public ObservableCollection<MonitoringActivity> Activities
        {
            get { return SelectedUser == null ? null : LoadActitvities(SelectedUser); }
        }

        public MonitoringUser SelectedUser
        {
            get { return selectedUserValue; }
            set
            {
                SetProperty(ref selectedUserValue, value);
                RaiseNotification("Activities");
            }
        }

        private ObservableCollection<MonitoringActivity> LoadActitvities(MonitoringUser user)
        {
            var details = new ObservableCollection<MonitoringActivity>(readWriteDb.GetUserActivities(user.Id));
            return details;
        }


        private void StartHosting()
        {
            serviceHost = new ServiceHost(typeof(Library));
            serviceHost.Open();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Users = new ObservableCollection<MonitoringUser>(readWriteDb.GetUsersWithEventFromDb());
            if (Users.Count > 0)
                SelectedUser = users[0];
        }

        public void StartSaveEvent()
        {
            if (EventView.NameEvent.Length <= 0) return;
            if (EventView.ObserverEvent.FirstName.Length <= 0) return;
            if (EventView.ObserverEvent.LastName.Length <= 0) return;
            EventView.PropertyChanged += SetFinishEvent;
            readWriteDb = new ReadWriteDB(EventView, Attentions);
            readWriteDb.SaveEventWithObserverToDb();
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 1, 0); //interval v minutach
            dispatcherTimer.Start();
        }

        private void SetFinishEvent(object sender, PropertyChangedEventArgs e)
        {
            if (readWriteDb != null)
                readWriteDb.SaveEventFinishToDb();
        }
    }

    public class BindableBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
        {
            member = value;
            RaiseNotification(propertyName);
        }

        protected void RaiseNotification(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}