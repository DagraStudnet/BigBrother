using System;

namespace HostingBigBrother.Model
{
    public class Event
    {
        public int Id { get; set; }
        public string NameEvent { get; set; }
        public DateTime StarTimeEvent { get; set; }
        public DateTime EndTimeEvent { get; set; }
        public Observer ObserverEvent { get; set; }
    }
}