using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using ClassLibrary;
using UserStorageLibrary;
using HostingBigBrother.Model;

namespace HostingBigBrother.ViewModel
{
    public class ViewModelMain
    {
        private readonly UserNDatabase userNDatabase;
        private readonly UserCollection userCollection;

        public ViewModelMain()
        {
            userNDatabase = UserNDatabase.ReturnDatabaseInstance();
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 1, 0); //interval v minutach
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var userCollectionFromDb = GetCollectionUsersFormDb();
            var userTransformCollection =
                userCollectionFromDb.Select(TransformerUsersFromDbStorage.UserTransform).ToList();
            SetTransformedCollection(userTransformCollection);
        }

        private void SetTransformedCollection(IEnumerable<MonitoringUser> userTransformCollection)
        {
            userCollection.AddRangeUser(userTransformCollection);
        }

        public IEnumerable<IUser> GetCollectionUsersFormDb()
        {
           return  userNDatabase.GetCollectionUsersFromDB();
            
        }
    }
}
