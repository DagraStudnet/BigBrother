
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
    
public partial class Db_user_date_time_event
{

    public long id_user_date_time_event { get; set; }

    public string name_work { get; set; }

    public Nullable<long> id_user { get; set; }

    public Nullable<long> id_date_time_event { get; set; }



    public virtual Db_date_time_event Db_date_time_event { get; set; }

    public virtual Db_user Db_user { get; set; }

}

}
