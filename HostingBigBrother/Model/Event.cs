using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HostingBigBrother.Annotations;

namespace HostingBigBrother.Model
{
    public class Event : INotifyPropertyChanged
    {
        public Event()
        {
            ObserverEvent = new Observer();
        }
        public int Id { get; set; }

        public string NameEvent
        {
            get { return _nameEvent; }
            set
            {
                _nameEvent = value;
                OnPropertyChanged();
            }
        }

        public DateTime StarTimeEvent { get; set; }

        private DateTime endTimeEvent;
        private string _nameEvent;

        public DateTime EndTimeEvent
        {
            get { return endTimeEvent; }
            set
            {
                {
                    endTimeEvent = value;
                    OnPropertyChanged();
                }
            }
        }

        public Observer ObserverEvent { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}