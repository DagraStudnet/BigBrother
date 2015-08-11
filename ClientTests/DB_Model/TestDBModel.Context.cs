﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Data.Entity.ModelConfiguration.Conventions;

namespace ClientTests.DB_Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TestingCloneEntities : DbContext
    {
        public TestingCloneEntities()
            : base("name=TestingCloneEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    
        public virtual DbSet<Db_activity> Db_activity { get; set; }
        public virtual DbSet<Db_date_time_event> Db_date_time_event { get; set; }
        public virtual DbSet<Db_event> Db_event { get; set; }
        public virtual DbSet<Db_observer> Db_observer { get; set; }
        public virtual DbSet<Db_user> Db_user { get; set; }
        public virtual DbSet<Db_user_work> Db_user_work { get; set; }
    }
}
