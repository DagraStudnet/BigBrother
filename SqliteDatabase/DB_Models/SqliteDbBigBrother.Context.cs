﻿

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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class BigBrotherEntities : DbContext
{
    public BigBrotherEntities()
        : base("name=BigBrotherEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<Db_activity> Db_activity { get; set; }

    public virtual DbSet<Db_date_time_event> Db_date_time_event { get; set; }

    public virtual DbSet<Db_event> Db_event { get; set; }

    public virtual DbSet<Db_observer> Db_observer { get; set; }

    public virtual DbSet<Db_user> Db_user { get; set; }

    public virtual DbSet<Db_user_date_time_event> Db_user_date_time_event { get; set; }

}

}

