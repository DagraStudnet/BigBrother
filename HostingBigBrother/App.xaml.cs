using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ClientBigBrother;

namespace BigBrotherViewer
{
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
    public partial class App
    {
        [STAThread]
        public static void Main()
        {
            var application = new App();
            application.InitializeComponent();
            application.Run();
        }


        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}\n", e.Exception.Message);
            stringBuilder.AppendFormat("{0}\n", e.Exception.InnerException.Message);
            stringBuilder.AppendFormat(
                    "Exception handled on main UI thread {0}.", e.Dispatcher.Thread.ManagedThreadId);
         
            MessageBox.Show("Application must exit:\n\n" + stringBuilder.ToString(),
                            "app",MessageBoxButton.OK,MessageBoxImage.Error);
            
            this.Shutdown(0);
            
            e.Handled = true;
        }
    }
}
