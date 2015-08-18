using System.Windows;
using HostingBigBrother.Model;
using HostingBigBrother.ViewModel;

namespace HostingBigBrother.View
{
    /// <summary>
    ///     Interaction logic for EventView.xaml
    /// </summary>
    public partial class EventView : Window
    {

        public EventView(ViewModelMain viewModelMain)
        {
            InitializeComponent();
            DataContext = viewModelMain;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void StornoButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        
    }
}