using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BigBrotherViewer.Model;
using BigBrotherViewer.ViewModel;

namespace BigBrotherViewer.View
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DateTime _time;
        private AttentionsView attentionsView;
        private ListSortDirection columnSortingDescription;
        private int columnSortingIndex;
        private EventView eventView;
        private HistoricalEventView historicalEventView;
        private ViewModelMain main;

        public MainWindow()
        {
            InitializeComponent();
        }

        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            main = new ViewModelMain();
            if (main.ConfigFileDoesntWork) Exit_Click(sender, e);
            DataContext = main;
            StopEventMenu.IsEnabled = false;
            ((INotifyCollectionChanged) UserActivitiesDataGrid.Items).CollectionChanged += OnCollectionChanged;
            UsersDataGrid.SelectionChanged += UsersDataGridOnSelectionChanged;
            UsersDataGrid.PreparingCellForEdit += UsersDataGridOnPreparingCellForEdit;
            UsersDataGrid.RowEditEnding += UsersDataGridOnRowEditEnding;
             ((INotifyCollectionChanged) UsersDataGrid.Items).CollectionChanged += UserOnCollectionChanged;
        }

        private void UserOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if(main.SelectedUser == null)return;
            var index = UsersDataGrid.Items.Cast<MonitoringUser>().ToList().FindIndex(u => u.Id == main.SelectedUser.Id);
            UsersDataGrid.SelectedIndex = index;
            UsersDataGrid.Focus();

        }

        private void UsersDataGridOnRowEditEnding(object sender,
            DataGridRowEditEndingEventArgs dataGridRowEditEndingEventArgs)
        {
            main.EditingMode = false;
        }

        private void UsersDataGridOnPreparingCellForEdit(object sender,
            DataGridPreparingCellForEditEventArgs dataGridPreparingCellForEditEventArgs)
        {
            main.EditingMode = true;
        }


        private void UsersDataGridOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            main.SelectedUser = (UsersDataGrid.SelectedItem as MonitoringUser);
        }


        private void OnCollectionChanged(object sender,
            NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (UserActivitiesDataGrid.Items.Count == 0)
            {
                return;
            }
            UserActivitiesDataGrid.ScrollIntoView(UserActivitiesDataGrid.Items[UserActivitiesDataGrid.Items.Count - 1]);
        }

        private void Add_attentions_Click(object sender, RoutedEventArgs e)
        {
            attentionsView = new AttentionsView(main.Attentions);
            attentionsView.ShowDialog();
            if (attentionsView.DialogResult == false) return;
            string text = attentionsView.TextBoxAttentions.Text;
            if (text.Length <= 0)
            {
                main.Attentions.Clear();
                return;
            }
            List<string> attentionNameList = text.Split(',').ToList();
            int index = 0;
            do
            {
                index = attentionNameList.FindIndex(a => a == string.Empty);
                if (index > -1) attentionNameList.RemoveAt(index);
            } while (index > -1);

            List<Attention> attentions =
                attentionNameList.Select(attentionName => new Attention {Name = attentionName}).ToList();
            main.Attentions = attentions;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (main.ConfigFileDoesntWork) return;
            main.SaveAttetions();
            if (main.EventView == null)
            {
                MessageBoxResult resultMessage = MessageBox.Show("Do you want finish application.", "Finish application",
                    MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (resultMessage == MessageBoxResult.Yes)
                    return;
                e.Cancel = true;
            }
            else if (main.EventView != null)
            {
                MessageBoxResult result = GetResultMessageBox();
                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                if (main.EventView.NameEvent != string.Empty)
                    main.EventView.EndTimeEvent = GetDateTimeNow();
            }
        }

        private void Add_event_Click(object sender, RoutedEventArgs e)
        {
            if (main.EventView == null)
            {
                CreateEvent();
            }
        }

        private static MessageBoxResult GetResultMessageBox()
        {
            return MessageBox.Show("Do you want to finish this event? Pres button OK.",
                "Finish event", MessageBoxButton.OKCancel,
                MessageBoxImage.Stop);
        }

        private void CreateEvent()
        {
            eventView = new EventView();
            eventView.ShowDialog();
            if (eventView.DialogResult == false) return;
            main.EventView = new Event
            {
                NameEvent = eventView.NameEvent,
                StarTimeEvent = GetDateTimeNow(),
                ObserverEvent =
                    new Observer {FirstName = eventView.FirstNameObserver, LastName = eventView.LastNameObserver}
            };
            main.StartSaveEvent();
            CreateEventMenu.IsEnabled = false;
            StopEventMenu.IsEnabled = true;
        }

        private static DateTime GetDateTimeNow()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour,
                DateTime.Now.Minute, DateTime.Now.Second);
        }

        private void Add_Historical_Event_Click(object sender, RoutedEventArgs e)
        {
            historicalEventView = new HistoricalEventView(main.Attentions);
            Hide();
            historicalEventView.ShowDialog();
            Show();
        }

        private void Stop_event_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultMessageBox = GetResultMessageBox();
            if (resultMessageBox != MessageBoxResult.OK) return;
            CreateEventMenu.IsEnabled = true;
            StopEventMenu.IsEnabled = false;
            main.StopEvent();
            main.EventView.EndTimeEvent = GetDateTimeNow();
            main.EventView = null;
            main.Users = new ObservableCollection<MonitoringUser>();
            main.SelectedUser = null;
        }
        
    }
}