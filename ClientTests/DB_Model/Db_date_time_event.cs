//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientTests.DB_Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Db_date_time_event
    {
        public long id_date_time_event { get; set; }
        public string start_event { get; set; }
        public string end_event { get; set; }
        public long id_observer { get; set; }
        public long id_event { get; set; }
        public Nullable<long> id_user { get; set; }
    
        public virtual Db_event Db_event { get; set; }
        public virtual Db_user Db_user { get; set; }
        public virtual Db_observer Db_observer { get; set; }
    }
}
