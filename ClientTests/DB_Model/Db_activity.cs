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
    
    public partial class Db_activity
    {
        public long id_activity { get; set; }
        public string name { get; set; }
        public string time_activity { get; set; }
        public Nullable<bool> attention { get; set; }
        public Nullable<long> id_user { get; set; }
    
        public virtual Db_user Db_user { get; set; }
    }
}
