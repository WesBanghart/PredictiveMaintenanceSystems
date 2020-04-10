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

        public EFSystemContext(DbContextOptions<EFSystemContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Tenant Table Relationships
            modelBuilder.Entity<TenantTable>()
                .HasMany<UserTable>(t => t.Users)
                .WithOne(u => u.Tenant)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // User Table Relationships
            modelBuilder.Entity<UserTable>()
                .HasMany<ModelTable>(u => u.Models)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserTable>()
                .HasMany<SchedulerTable>(u => u.Schedulers)
                .WithOne(s => s.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserTable>()
                .HasMany<DataSourceTable>(u => u.DataSources)
                .WithOne(ds => ds.User)
                .HasForeignKey(ds => ds.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
