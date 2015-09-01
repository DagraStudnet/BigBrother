﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Windows.Threading;
using BigBrotherViewer.Model;
using WcfServiceLibrary;

namespace BigBrotherViewer.ViewModel
{
    public class ViewModelMain : BindableBase, INotifyPropertyChanged
    {
        private ReadWriteDB readWriteDb;

        private ServiceHost serviceHost;

        private ObservableCollection<MonitoringUser> users;
        private MonitoringUser selectedUserValue;
        private List<Attention> _attentions;
        private Event _eventView;
        private string _fillNameActivity;
        private bool _onlyAttentions;

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

        public Event EventView
        {
            get { return _eventView; }
            set
            {
                SetProperty(ref _eventView, value);
                RaiseNotification("EvenView");
            }
        }

        public ViewModelMain()
        {
            
            Attentions = new List<Attention>();
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
            var userActivities = ReturnUserActivity(user);
            if (OnlyAttentions)
                userActivities = userActivities.Where(a => a.Attention).ToList();
            if (!string.IsNullOrEmpty(FillNameActivity))
                userActivities = userActivities.Where(a => a.NameActivity.Contains(FillNameActivity)).ToList();
            return new ObservableCollection<MonitoringActivity>(userActivities);
        }

        private IEnumerable<MonitoringActivity> ReturnUserActivity(MonitoringUser user)
        {
            return readWriteDb.GetUserActivities(user.Id);
        }

        public string FillNameActivity
        {
            get { return _fillNameActivity; }
            set
            {
                SetProperty(ref _fillNameActivity, value);
                RaiseNotification("Activities");
            }
        }

        public bool OnlyAttentions
        {
            get { return _onlyAttentions; }
            set
            {
                SetProperty(ref _onlyAttentions, value);
                RaiseNotification("Activities");
            }
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
            StartHosting();
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
            serviceHost.Close();
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