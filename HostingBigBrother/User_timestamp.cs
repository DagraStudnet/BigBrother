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
    
    public partial class User_timestamp
    {
        public long id_user_timestamp { get; set; }
        public string user_timestamp { get; set; }
        public Nullable<long> id_user { get; set; }
    
        public virtual User User { get; set; }
    }
}