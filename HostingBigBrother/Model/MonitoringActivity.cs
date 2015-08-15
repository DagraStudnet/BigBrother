using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClassLibrary;
using ClassLibrary.UserLibrary;
using HostingBigBrother.Annotations;

namespace HostingBigBrother.Model
{
    public class MonitoringActivity : IActivity, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public bool Attention { get; set; }

        private bool _ignoreAttention;
        public bool IgnoreAttention
        {
            get { return _ignoreAttention; }
            set
            {
                _ignoreAttention = value;
                OnPropertyChanged();
            }
        }
        public string NameActivity { get; set; }
        public DateTime TimeActivity { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}