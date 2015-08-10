using System;
using System.Collections.Generic;
using System.Windows.Threading;
using HostingBigBrother.Model;


namespace HostingBigBrother.ViewModel
{
    public class ViewModelMain
    {
        //private readonly UserStorage userNDatabase;
        private readonly UserCollection userCollection;

        public ViewModelMain()
        {
            //userNDatabase = UserStorage.ReturnDatabaseInstance();
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 1, 0); //interval v minutach
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
        //    var userCollectionFromDb = GetCollectionUsersFormDb();
        //    var userTransformCollection =
        //        userCollectionFromDb.Select(TransformerUsersFromNdbStorage.UserTransform).ToList();
        //    SaveTransformedCollection(userTransformCollection);
        }

        private void SaveTransformedCollection(IEnumerable<MonitoringUser> userTransformCollection)
        {
            userCollection.AddRangeUser(userTransformCollection);
        }
    }
}
