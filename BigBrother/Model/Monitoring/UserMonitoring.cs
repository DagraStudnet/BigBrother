using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using ClassLibrary;
using ClientBigBrother.ViewModel;

namespace ClientBigBrother.Model.Monitoring
{
    public class UserMonitoring<T> : IUserMonitoring<T>
        where T:IUser
    {
        private readonly List<string> listUSB = new List<string>();
        private readonly StringBuilder nameActivities = new StringBuilder(255);
        private string previousName = string.Empty;
        
        public void SaveInformationAboutUserPc(T user)
        {
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            string[] text = windowsIdentity.Name.Split('\\'); // metoda mi da nazev pc a uzivatele
            user.PCName = text[0];
            user.UserName = text[1];
           
        }

        public void SaveUsbConnection(T user)
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
                    user.ListOfActivitesOnPc.Add(CreateActivity(string.Format("USB-{0}", driver)));
                }
            }
        }

        public void SaveOpenProgramUser(T user)
        {
            WindowsApiFunction.GetWindowText(WindowsApiFunction.GetForegroundWindow(), nameActivities,
                nameActivities.Capacity);
            if (nameActivities.ToString() != previousName)
            {
                user.ListOfActivitesOnPc.Add(CreateActivity(nameActivities.ToString()));
                previousName = nameActivities.ToString();
                nameActivities.Clear();
            }
        }

        public void SaveStartUpProgramsOnDestop(T user)
        {
            foreach (
               Process process in
                   Process.GetProcesses().Where(process => !string.IsNullOrEmpty(process.MainWindowTitle)))
            {
                user.ListOfActivitesOnPc.Add(CreateActivity(process.MainWindowTitle));
            }
        }

        private static Activity CreateActivity(string nameActivity)
        {
            return new Activity { NameActivity = string.Format("{0}", nameActivity), TimeActivity = DateTime.Now };
        }
    }
}