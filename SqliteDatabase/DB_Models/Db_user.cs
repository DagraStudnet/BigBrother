//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SqliteDatabase.DB_Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Db_user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Db_user()
        {
            this.Db_activity = new HashSet<Db_activity>();
            this.Db_user_date_time_event = new HashSet<Db_user_date_time_event>();
        }
    
        public long id_user { get; set; }
        public string user_name { get; set; }
        public string pc_name { get; set; }
        public string user_timestamp { get; set; }
        public Nullable<int> attention { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Db_activity> Db_activity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Db_user_date_time_event> Db_user_date_time_event { get; set; }
    }
}
