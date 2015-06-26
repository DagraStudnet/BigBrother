using System.Collections.Generic;
using System.Runtime.Serialization;
using ClassLibrary;

namespace ClientBigBrother.Model.PcUser
{
    [DataContract]
    public class UserContract : User
    {
        public UserContract()
        {
            ListOfActivitesOnPc = new List<Activity>();
        }

        public void ClearActivites()
        {
            ListOfActivitesOnPc.Clear();
        }
    }
}
