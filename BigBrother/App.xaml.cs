using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace ClientBigBrother
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
    public partial class App : ISingleInstanceApp
    {
        private const string Unique = "Change this to something that uniquely identifies your program.";

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            if (this.MainWindow.WindowState == WindowState.Minimized)
            {
                this.MainWindow.WindowState = WindowState.Normal;
            }

            this.MainWindow.Activate();

            return true;
        }

        [STAThread]
        public static void Main()
        {
                var currentDomain = AppDomain.CurrentDomain;
                currentDomain.UnhandledException += MyHandler;
                if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
                {

                    var application = new App();
                    application.InitializeComponent();
                    application.Run();

                    // Allow single instance code to perform cleanup operations
                    SingleInstance<App>.Cleanup();
                }
        }

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var e = (Exception)args.ExceptionObject;
            var sb = new StringBuilder();
            sb.Append("Error message : " + e.Message);
            sb.Append("Inner error message : " + e.InnerException.Message);
            MessageBox.Show(sb.ToString(), "Erreur", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }
    }
}