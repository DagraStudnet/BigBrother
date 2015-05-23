using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using ClassLibrary;
using ClientBigBrother.Model;

namespace ClientBigBrother.ViewModel
{
    public class MananingUser
    {
        private readonly List<string> listUSB = new List<string>();
        private readonly StringBuilder nameActivities;
        private string previousName = string.Empty;

        public User User { get; private set; }

        public MananingUser()
        {
            nameActivities = new StringBuilder(255);
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            if (windowsIdentity == null) return;
            string[] text = windowsIdentity.Name.Split('\\'); // metoda mi da nazev pc a uzivatele
            User = new UserContract()
            {
                PCName = text[0],
                UserName = text[1],
            };
        }
        
        public void SaveConnectionUsb()
        {
            if (DriveInfo.GetDrives().All(d => d.DriveType != DriveType.Removable))
            {
                listUSB.Clear();
            }
            else
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                IEnumerable<DriveInfo> drivesRemovable = drives.Where(d => d.DriveType == DriveType.Removable);
                IEnumerable<string> drivesIsNotConstainInFieldUsb = drivesRemovable.Select(d => d.Name + d.VolumeLabel).
                    Where(
                        driver =>
                            driver.ToString() !=
                            listUSB.Find(x => x.Contains(driver.ToString(CultureInfo.InvariantCulture))));
                foreach (string driver in drivesIsNotConstainInFieldUsb)
                {
                    listUSB.Add(driver);
                    User.ListOfActivitesOnPc.Add(CreateActivity(string.Format("USB-{0}", driver)));
                }
            }
        }

        private static Activity CreateActivity(string nameActivity)
        {
            return new Activity {NameActivity = string.Format("{0}", nameActivity), TimeActivity = DateTime.Now};
        }

        public void StartUpApplication()
        {
            foreach (
                Process process in
                    Process.GetProcesses().Where(process => !string.IsNullOrEmpty(process.MainWindowTitle)))
            {
                User.ListOfActivitesOnPc.Add(CreateActivity(process.MainWindowTitle));
            }
        }

        public void SaveUserActivities()
        {
            WindowsApiFunction.GetWindowText(WindowsApiFunction.GetForegroundWindow(), nameActivities,
                nameActivities.Capacity);
            if (nameActivities.ToString() == previousName) return;
            User.ListOfActivitesOnPc.Add(CreateActivity(nameActivities.ToString()));
            previousName = nameActivities.ToString();
            nameActivities.Clear();
        }

        public void ClearUserActivites()
        {
            User.ListOfActivitesOnPc.Clear();
        }
    }
}