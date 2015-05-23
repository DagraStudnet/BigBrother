using System.ServiceModel;
using System.Windows;
using UserStorageLibrary;
using WcfServiceLibrary;

namespace HostingBigBrother
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UserNDatabase userSingleton;
        private ServiceHost host;

        public MainWindow()
        {
            InitializeComponent();
            userSingleton = UserNDatabase.ReturnDatabaseInstance();
        }

        private void OpenConnectionWcf()
        {
            host = new ServiceHost(typeof (Library));
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