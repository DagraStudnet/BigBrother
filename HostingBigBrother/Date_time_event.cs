//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HostingBigBrother
{
    using System;
    using System.Collections.Generic;
    
    public partial class Date_time_event
    {
        public long id_date_time_event { get; set; }
        public string start_event { get; set; }
        public string end_event { get; set; }
        public long id_observer { get; set; }
        public long id_event { get; set; }
        public Nullable<long> id_user { get; set; }
    
        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
        public virtual Observer Observer { get; set; }
    }
}