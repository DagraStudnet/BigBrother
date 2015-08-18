using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using HostingBigBrother.Annotations;


namespace HostingBigBrother.View
{
    /// <summary>
    ///     Interaction logic for EventView.xaml
    /// </summary>
    public partial class EventView : Window,INotifyPropertyChanged
    {
        private string _nameEvent;
        private string _firstNameObserver;
        private string _lastNameObserver;

        public string NameEvent
        {
            get { return _nameEvent; }
            set
            {
                if (value == _nameEvent) return;
                _nameEvent = value;
                OnPropertyChanged();
            }
        }

        public string FirstNameObserver
        {
            get { return _firstNameObserver; }
            set
            {
                if (value == _firstNameObserver) return;
                _firstNameObserver = value;
                OnPropertyChanged();
            }
        }

        public string LastNameObserver
        {
            get { return _lastNameObserver; }
            set
            {
                if (value == _lastNameObserver) return;
                _lastNameObserver = value;
                OnPropertyChanged();
            }
        }

        public EventView()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void StornoButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}