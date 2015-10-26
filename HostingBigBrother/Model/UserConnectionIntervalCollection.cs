using System;
using System.Collections.Generic;
using SqliteDatabase.DB_Models;

namespace BigBrotherViewer.Model
{
    public class UserConnectionIntervalCollection<T> where T : UserConnectionInterval, new()
    {
        public UserConnectionIntervalCollection()
        {
            UserConnectionIntervalList = new List<T>();
        }

        public List<T> UserConnectionIntervalList { get; set; }

        public void AddUser(MonitoringUser user)
        {
            var findUserConnection = UserConnectionIntervalList.Find(userid => user.Id == userid.Id);
            if (findUserConnection != null)
                findUserConnection.SetUserConnectionInterval(user.TimeStampDispatch);
            else
                UserConnectionIntervalList.Add(new T
                {
                    Id = user.Id,
                    NewSendDateTime = user.TimeStampDispatch
                });
        }

        public int GetInterval(int userId)
        {
            var findUserConnection = UserConnectionIntervalList.Find(userid => userId == userid.Id);
            return findUserConnection.PreviousSendDateTime == null
                ? GetSecondBetweenTwoDateTime(findUserConnection.NewSendDateTime,
                    findUserConnection.NewSendDateTime.AddDays(1))
                : GetSecondBetweenTwoDateTime(findUserConnection.NewSendDateTime,
                    findUserConnection.PreviousSendDateTime.Value);
        }

        private static int GetSecondBetweenTwoDateTime(DateTime fistDateTime, DateTime seconDateTime)
        {
            return Math.Abs((int) (fistDateTime - seconDateTime).TotalSeconds);
        }
    }
}