using System.ServiceModel;
using System.Windows;
using UserStorageNDatabase;
using WcfServiceLibrary;


namespace HostingBigBrother
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UserStorage userSingleton;
        private ServiceHost host;

        public MainWindow()
        {
            InitializeComponent();
            userSingleton = UserStorage.ReturnDatabaseInstance();
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
            foreach (var listUser in userSingleton.GetCollectionUsersFromDB())
            {
                V.Items.Add(listUser.UserName);
                foreach (var activites in listUser.ListOfActivitesOnPc)
                {
                    V.Items.Add(activites.NameActivity);
                }
            }
        }

        private void Pripojit_Click(object sender, RoutedEventArgs e)
        {
            OpenConnectionWcf();
        }
    }
}