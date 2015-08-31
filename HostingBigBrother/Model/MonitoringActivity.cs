using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BigBrotherViewer.Annotations;
using ClassLibrary;
using ClassLibrary.UserLibrary;

namespace BigBrotherViewer.Model
{
    public class MonitoringActivity : IActivity, INotifyPropertyChanged
    {
        public int Id { get; set; }

        private bool attention;
        public bool Attention
        {
            get { return attention; }
            set
            {
                attention = value;
                MarkRow = !IgnoreAttention && Attention;
                OnPropertyChanged();
            }
        }


        private bool _ignoreAttention;
        public bool IgnoreAttention
        {
            get { return _ignoreAttention; }
            set
            {
                _ignoreAttention = value;
                MarkRow = !IgnoreAttention && Attention;
                OnPropertyChanged();
            }
        }
        public string NameActivity { get; set; }
        public DateTime TimeActivity { get; set; }
        private bool markRow;
        public bool MarkRow
        {
            get { return markRow; }
            set
            {
                markRow = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}