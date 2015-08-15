using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using HostingBigBrother.Annotations;
using HostingBigBrother.Model;


namespace HostingBigBrother.ViewModel
{
    public class ViewModelMain : BindableBase, INotifyPropertyChanged
    {
        //private readonly UserStorage userNDatabase;
        //private readonly MonitoringUsersCollection _monitoringUsersCollection;
        public Event EventView { get; set; }

        private ReadWriteDB readWriteDb;

        private ObservableCollection<MonitoringUser> users;
        public ObservableCollection<MonitoringUser> Users
        { //get; set; }
            get { return users; }
            set
            {
                SetProperty(ref users, value);
                RaiseNotification("Users");
            }
        }
        
        private ObservableCollection<MonitoringActivity> activities;
        public ObservableCollection<MonitoringActivity> Activities
        {
            get
            {
                if (SelectedUser == null)
                {
                    return null;
                }
                return LoadActitvities(SelectedUser);
            }

        }

        private MonitoringUser selectedUserValue;
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


        public ViewModelMain()
        {
            GetEvent();
            this.readWriteDb = new ReadWriteDB(EventView);
            readWriteDb.SaveEventWithObserverToDb();
            readWriteDb.GetEventId();
            readWriteDb.SaveRelationshipBetweenEventAndUsers();
            Users = new ObservableCollection<MonitoringUser>(readWriteDb.GetUsersWithEventFromDb());
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 1, 0); //interval v minutach
            dispatcherTimer.Start();

        }



        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //    var userCollectionFromDb = GetCollectionUsersFormDb();
            //    var userTransformCollection =
            //        userCollectionFromDb.Select(TransformerUsersFromNdbStorage.UserTransform).ToList();
            //    SaveTransformedCollection(userTransformCollection);
        }

        private void GetEvent()
        {
            EventView = new Event()
            {
                NameEvent = "Zkouska",
                StarTimeEvent = new DateTime(2015, 1, 1),
                ObserverEvent = new Observer() { LastName = "Rajm", FirstName = "Lukas" }
            };
        }


        public void ReloadUsers()
        {
            Users = new ObservableCollection<MonitoringUser>(readWriteDb.GetUsersWithEventFromDb());
        }

    }
    public class BindableBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
        {
            member = value;
            this.RaiseNotification(propertyName);
        }

        protected void RaiseNotification(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class Refresher : ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            //Your Code
        }
        #endregion
    }

}
