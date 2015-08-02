using System.Windows;
using System.Windows.Media;
using ClientBigBrother.ViewModel;

namespace ClientBigBrother.View
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModelMain vmMain;

        public MainWindow()
        {
            InitializeComponent();
            vmMain = new ViewModelMain();
            if(vmMain.ConfigFileDoesntWork)
                Close();
        }
    }
}