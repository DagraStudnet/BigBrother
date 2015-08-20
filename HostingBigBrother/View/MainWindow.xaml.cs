using System;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using HostingBigBrother.Model;
using HostingBigBrother.ViewModel;
using SqliteDatabase;

namespace HostingBigBrother.View
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModelMain main;
        private AttentionsView attentionsView;
        private EventView eventView;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            main = new ViewModelMain();
            DataContext = main;
        }

        private void Add_attentions_Click(object sender, RoutedEventArgs e)
        {
            attentionsView = new AttentionsView(main.Attentions);
            attentionsView.ShowDialog();
            if (attentionsView.DialogResult == false) return;
            var text = attentionsView.TextBoxAttentions.Text;
            if (text.Length <= 0) return;
            var attentionNameList = text.Split(',').ToList();
            int index = 0;
            do
            {
                index = attentionNameList.FindIndex(a => a == string.Empty);
                if (index > -1) attentionNameList.RemoveAt(index);
            } while (index > -1);

            var attentions = attentionNameList.Select(attentionName => new Attention { Name = attentionName }).ToList();
            main.Attentions = attentions;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Do you want to close this window?", "Finish event", MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            if (main.EventView == null) return;
            if (main.EventView.NameEvent != string.Empty)
                main.EventView.EndTimeEvent = GetDateTimeNow();
        }

        private void Add_event_Click(object sender, RoutedEventArgs e)
        {
            if (main.EventView == null)
            {
                CreateEvent();
                return;
            }
            var result = MessageBox.Show("Do you want to finish this event? Pres button OK.", "Finish event", MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                main.EventView.EndTimeEvent = GetDateTimeNow();
                main.Users = null;
                main.SelectedUser = null;
                CreateEvent();
            }
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
                ObserverEvent = new Observer { FirstName = eventView.FirstNameObserver, LastName = eventView.LastNameObserver }
            };
            main.StartSaveEvent();
        }

        private static DateTime GetDateTimeNow()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
    }
}