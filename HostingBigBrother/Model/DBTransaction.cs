using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.UserLibrary;

namespace HostingBigBrother.Model
{
    public class DBTransaction
    {
        private static DBTransaction dbTransaction;
        private const string DateTimeFormate = "YYYY-M-D DDDD HH:MM:SS";
        private DBTransaction()
        {}

        public static DBTransaction GetDbTransaction()
        {
            return dbTransaction ?? new DBTransaction();
        }

        public void AddUser(IUser user)
        {
            using (var context =  new BigBrotherDBEntities() )
            {
                var findUser =
                    context.Users.Single(u => u.user_name.Equals(user.UserName) && u.pc_name.Equals(user.PCName));
                if (findUser == null)
                    InsertUser(context,user);
                else
                {
                    UpdateUser(context,findUser,user);
                }
                context.SaveChanges();
            }
        }

        private void UpdateUser(BigBrotherDBEntities context, User findUseruser, IUser user)
        {
            foreach (var item in user.ListOfActivitesOnPc)
            {
                var activity = new Activity()
                {
                    User = findUseruser,
                    name = item.NameActivity,
                    time_activity = item.TimeActivity.ToString(DateTimeFormate),
                    attention = false
                };
                findUseruser.Activities.Add(activity);
                context.Activities.Add(activity);
            };
        }

        private void InsertUser(BigBrotherDBEntities context, IUser user)
        {
            var dbUser = new User()
            {
                Activities =  user.ListOfActivitesOnPc,
                pc_name = user.PCName,
                user_name = user.UserName
            };

            var userTimestamp = new User_timestamp()
            {
                user_timestamp = user.TimeStampsDispatch.ToString(DateTimeFormate),
                User = dbUser
            };

            context.Users.Add(dbUser);
            context.User_timestamp.Add(userTimestamp);
           
        }
    }
}
