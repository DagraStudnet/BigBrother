using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ClassLibrary.UserLibrary
{
    [DataContract]
    public class User : IUser
    {
        public User()
        {
            ListOfActivitesOnPc = new List<Activity>();
        }
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string PCName { get; set; }

        [DataMember]
        public DateTime TimeStampDispatch { get; set; }

        [DataMember]
        public IList<Activity> ListOfActivitesOnPc { get; set; }
    }
}