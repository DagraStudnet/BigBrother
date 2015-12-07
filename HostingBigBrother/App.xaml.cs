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
            try
            {
                var application = new App();
                AppDomain.CurrentDomain.UnhandledException += MyHandler;

                application.InitializeComponent();
                application.Run();
            }
            catch (Exception e)
            {
                var sb = new StringBuilder();
                sb.Append("Error message : " + e.Message + "\n" + e.InnerException.Message);
                MessageBox.Show(sb.ToString());
            }
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var exp =(Exception) e.ExceptionObject;
            var sb = new StringBuilder();
            sb.Append("Error message : " + exp.Message + "\n" + exp.InnerException.Message);
            MessageBox.Show(sb.ToString());
        }
    }
}
