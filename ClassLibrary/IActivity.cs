using System;

namespace ClassLibrary
{
    public interface IActivity
    {
        string NameActivity { get; set; }
        DateTime TimeActivity { get; set; }
    }
}