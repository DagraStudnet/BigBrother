﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BigBrotherDBEntities : DbContext
    {
        public BigBrotherDBEntities()
            : base("name=BigBrotherDBEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Date_time_event> Date_time_event { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Observer> Observers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<User_timestamp> User_timestamp { get; set; }
    }
}
