using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HostingBigBrother.Annotations;

namespace HostingBigBrother.View
{
    /// <summary>
    /// Interaction logic for EventView.xaml
    /// </summary>
    public partial class EventView : Window,INotifyPropertyChanged
    {
        private string eventName;
        public string EventNameProperty {
            get { return eventName; }
            set
            {
                eventName = value;
                OnPropertyChanged();
            }
        }

        private string observerFirstName;
        public string ObserverFirstNameProperty
        {
            get { return observerFirstName; }
            set
            {
                observerFirstName = value;
                OnPropertyChanged();
            }
        }

        private string observerLastName;
        public string ObserverLastNameProperty
        {
            get { return observerLastName; }
            set
            {
                observerLastName = value;
                OnPropertyChanged();
            }
        }
        public EventView()
        {
            InitializeComponent();
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

        private void EventName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

   
}
