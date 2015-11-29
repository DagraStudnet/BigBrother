using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ClientBigBrother;

namespace BigBrotherViewer
{
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
    public partial class App
    {
        private const string Unique = "Change this to something that uniquely identifies your program.";

        [STAThread]
        public static void Main()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;
            var application = new App();
            application.InitializeComponent();
            application.Run();

        }

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var e = (Exception)args.ExceptionObject;
            var sb = new StringBuilder();
            sb.Append("Error message : " + e.InnerException.Message);
            MessageBox.Show(sb.ToString());
        }
    }
}
