using System;
using System.Runtime.Serialization;

namespace ClassLibrary.UserLibrary
{
    [DataContract]
    public class Activity : IActivity
    {
        [DataMember]
        public string NameActivity { get; set; }
        [DataMember]
        public DateTime TimeActivity { get; set; }
    }
}