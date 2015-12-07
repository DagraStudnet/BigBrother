using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
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
                if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
                {
                    var application = new App();
                    AppDomain.CurrentDomain.UnhandledException += MyHandler;
                    application.InitializeComponent();
                    application.Run();
                    // Allow single instance code to perform cleanup operations
                    SingleInstance<App>.Cleanup();
                }
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var exp = (Exception)e.ExceptionObject;
            var sb = new StringBuilder();
            sb.Append("Error message : " + exp.Message + "\n" + exp.InnerException.Message);
            MessageBox.Show(sb.ToString());
        }
    }
}