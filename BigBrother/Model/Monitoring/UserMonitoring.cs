using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using ClassLibrary.UserLibrary;
using ClientBigBrother.Model.WindowsFunction;

namespace ClientBigBrother.Model.Monitoring
{
    public class UserMonitoring<T> : IUserMonitoring<T>
        where T : IUser
    {
        private readonly List<string> listUSB = new List<string>();
        private readonly StringBuilder nameActivities = new StringBuilder(255);
        private string previousName = string.Empty;

        /// <summary>
        ///     Metoda uklada informace o nazvu pc a nazvu uzivatele do instance user
        /// </summary>
        /// <param name="user">IUser</param>
        public void SaveInformationAboutUserPc(T user)
        {
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            string[] text = windowsIdentity.Name.Split('\\');
            user.PCName = Environment.MachineName;
            user.UserName = text[1];
        }

        /// <summary>
        ///     Metoda uklada informace o pripojenych usb a behem aplikace pripojovanych usb
        /// </summary>
        /// <param name="user"></param>
        public void SaveUsbConnection(T user)
        {
            if (DriveInfo.GetDrives().All(d => d.DriveType != DriveType.Removable))
            {
                listUSB.Clear();
            }
            else
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                IEnumerable<DriveInfo> drivesRemovable = drives.Where(d => d.DriveType == DriveType.Removable && d.IsReady);
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

        /// <summary>
        ///     Metoda uklada nazev aplikace se kterou uzivatel prave pracuje.
        /// </summary>
        /// <param name="user"></param>
        public void SaveNowRuningApplicationUser(T user)
        {
            WindowsApiFunction.GetWindowText(WindowsApiFunction.GetForegroundWindow(), nameActivities,
                nameActivities.Capacity);
            if (nameActivities.ToString() == previousName)  return; 
            if(nameActivities.ToString() == string.Empty || nameActivities.ToString() == "Big Brother") return;
            user.ListOfActivitesOnPc.Add(CreateActivity(nameActivities.ToString()));
            previousName = nameActivities.ToString();
            nameActivities.Clear();
        }

        /// <summary>
        ///     Metoda uklada nazvy aplikaci ketere jsou spusteny pri spusteni sledovaci aplikace.
        /// </summary>
        /// <param name="user"></param>
        public void SaveStartUpApplicationsOnDestop(T user)
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
            return new Activity {NameActivity = string.Format("{0}", nameActivity), TimeActivity = DateTime.Now};
        }
    }
}