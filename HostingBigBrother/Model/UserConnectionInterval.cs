using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBrotherViewer.Model
{
    public class UserConnectionInterval
    {
        public int Id { get; set; }
        public DateTime NewSendDateTime { get; set; }
        public DateTime? PreviousSendDateTime { get; set; }

        public UserConnectionInterval()
        {
            PreviousSendDateTime = null;
        }
        public void SetUserConnectionInterval(DateTime newSendDateTime)
        {
            if (NewSendDateTime != newSendDateTime)
                if (PreviousSendDateTime == null)
                    PreviousSendDateTime = NewSendDateTime;
            if (NewSendDateTime != newSendDateTime)
                PreviousSendDateTime = NewSendDateTime;
            NewSendDateTime = newSendDateTime;

        }

    }
}
