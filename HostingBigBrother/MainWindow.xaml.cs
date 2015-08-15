using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows;
using System.Windows.Data;
using HostingBigBrother.Model;
using HostingBigBrother.ViewModel;
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
        private ViewModelMain main;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            main=new ViewModelMain();
            DataContext = main;
        }

    }
}