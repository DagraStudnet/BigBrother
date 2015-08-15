using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClassLibrary;
using ClassLibrary.UserLibrary;
using HostingBigBrother.Annotations;


namespace HostingBigBrother.Model
{
    public class MonitoringUser:INotifyPropertyChanged
    {
        public int Id { get; set; }
        public bool Attention { get; set; }

        private string nameWork;
        public string NameWork {
            get { return nameWork; }
            set
            {
                nameWork = value;
                OnPropertyChanged();
            }
        }
        public string UserName { get; set; }
        public string PCName { get; set; }
        public DateTime TimeStampDispatch { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}