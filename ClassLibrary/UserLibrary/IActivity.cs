using System;

namespace ClassLibrary.UserLibrary
{
    public interface IActivity
    {
        string NameActivity { get; set; }
        DateTime TimeActivity { get; set; }
    }
}