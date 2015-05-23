using System.Collections.Generic;
using System.Runtime.Serialization;
using ClassLibrary;

namespace ClientBigBrother.Model
{
    [KnownType(typeof (User))]
    [DataContract]
    public class UserContract : User
    {
        public UserContract()
        {
            ListOfActivitesOnPc = new List<Activity>();
        }
    }
}
