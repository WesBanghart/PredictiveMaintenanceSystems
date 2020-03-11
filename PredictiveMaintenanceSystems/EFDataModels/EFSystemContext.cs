using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFDataModels
{
    public class EFSystemContext : DbContext
    {
        //The Tenant table entries
        public DbSet<TenantTable> Tenants { get; set; }
        //The User table entries
        public DbSet<UserTable> Users { get; set; }
        //The Model table entries
        public DbSet<ModelTable> Models { get; set; }
        //The DataSource table entries
        public DbSet<DataSourceTable> DataSources { get; set; }
        //The Scheduler table entries
        public DbSet<SchedulerTable> Schedulers { get; set; }

        public EFSystemContext() { }

        public EFSystemContext(DbContextOptions<EFSystemContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //User Table Relationsips
            //User => Models
            modelBuilder.Entity<ModelTable>().HasOne(m => m.User).WithMany(u => u.Models).HasForeignKey(m => m.UserId);
            //User => DataSources
            modelBuilder.Entity<DataSourceTable>().HasOne(ds => ds.User).WithMany(u => u.DataSources).HasForeignKey(ds => ds.UserId);
            //User => Scedulers
            modelBuilder.Entity<SchedulerTable>().HasOne(s => s.User).WithMany(u => u.Schedulers).HasForeignKey(s => s.UserId);
           
            //Tenant Table Relationships
            //Tenant => Users
            modelBuilder.Entity<UserTable>().HasOne(u => u.Tenant).WithMany(t => t.Users).HasForeignKey(u => u.TenantId);
            //Tenant => Models
            modelBuilder.Entity<ModelTable>().HasOne(m => m.Tenant).WithMany(t => t.Models).HasForeignKey(m => m.TenantId);
            //Tenant => Schedulers
            modelBuilder.Entity<SchedulerTable>().HasOne(s => s.Tenant).WithMany(t => t.Schedulers).HasForeignKey(s => s.TenantId);

            //Model Table Relationships - Many to Many
            //Model => Datasources

            modelBuilder.Entity<DataSourceTable>().HasMany(m => m.Models);

            //Data Source Table Relationships - Many to Many
            //DataSources => Models

            modelBuilder.Entity<ModelTable>().HasMany(ds => ds.DataSources);
        }
    }
}
