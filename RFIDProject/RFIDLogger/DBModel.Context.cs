﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RFIDLogger
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RFID_DBEntities : DbContext
    {
        public RFID_DBEntities()
            : base("name=RFID_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<EntryLog> EntryLog { get; set; }
        public virtual DbSet<RfIdCard> RfIdCard { get; set; }
        public virtual DbSet<RfIdReader> RfIdReader { get; set; }
        public virtual DbSet<RfIdUser> RfIdUser { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}
