using System;
using SqliteDatabase.DB_Models;

namespace HostingBigBrother.Model
{
    public static class TransformationValuesFromDatabase
    {
        public static MonitoringUser TransformUserFromDB(Db_user dbUser)
        {
            return new MonitoringUser
            {
                Id = (int) dbUser.id_user,
                Attention = Convert.ToBoolean(dbUser.attention),
                PCName = dbUser.user_name,
                UserName = dbUser.user_name,
                TimeStampDispatch = DateTime.Parse(dbUser.user_timestamp)
            };
        }

        public static MonitoringActivity TransformActivityFromDB(Db_activity dbActivity)
        {
            return new MonitoringActivity
            {
                Id = (int) dbActivity.id_activity,
                NameActivity = dbActivity.name,
                TimeActivity = DateTime.Parse(dbActivity.time_activity),
                IgnoreAttention = Convert.ToBoolean(dbActivity.ignore_attention)
                
            };
        }
    }
}