using System.ServiceModel;
using System.Windows;
using SqliteDatabase;
using WcfServiceLibrary;


namespace HostingBigBrother
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DBTransaction dbTransactionSingleton;
        private ServiceHost host;

        public MainWindow()
        {
            InitializeComponent();
            dbTransactionSingleton = DBTransaction.ReturnDatabaseInstance();
        }

        private void OpenConnectionWcf()
        {
            host = new ServiceHost(typeof(Library));
            V.Items.Add(host.State.ToString());
            host.Open();
            V.Items.Add(host.State.ToString());
        }


        private void VypsatButton_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void Pripojit_Click(object sender, RoutedEventArgs e)
        {
            OpenConnectionWcf();
        }
    }
}