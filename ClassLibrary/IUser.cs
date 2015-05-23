using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    public interface IUser
    {
        string UserName { get; set; }
        string PCName { get; set; }
        DateTime TimeStampsDispatch { get; set; }
        IList<Activity> ListOfActivitesOnPc { get; set; }
    }
}