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
        private readonly DBTransaction dbTransactionSingleton;
        private ServiceHost host;
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
            if (attentionsView.DialogResult == null) return;
            var text = attentionsView.TextBoxAttentions.Text;
            if (text.Length <= 0) return;
            var attentionNameList = text.Split(',').ToList();
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
            if (main.EventView != null)
            {
                main.EventView.EndTimeEvent = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            }
        }

        private void Add_event_Click(object sender, RoutedEventArgs e)
        {
            eventView = new EventView();
            eventView.ShowDialog();
        }
    }
}