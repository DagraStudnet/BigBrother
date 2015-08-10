using System;
using System.Collections.Generic;

namespace ClassLibrary.UserLibrary
{
    public interface IUser
    {
        string UserName { get; set; }
        string PCName { get; set; }
        DateTime TimeStampDispatch { get; set; }
        IList<Activity> ListOfActivitesOnPc { get; set; }
    }
}