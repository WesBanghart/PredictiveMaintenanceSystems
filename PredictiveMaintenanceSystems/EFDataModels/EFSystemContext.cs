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
            ////User Table Relationsips
            modelBuilder.Entity<UserTable>()
                .HasMany(u => u.Models)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserTable>()
                .HasMany(u => u.DataSources)
                .WithOne(ds => ds.User)
                .HasForeignKey(ds => ds.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserTable>()
                .HasMany(u => u.Schedulers)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            /////////////////////////////////////////////////////////////////////////////////
            // Tenant Table Relationships
            modelBuilder.Entity<TenantTable>()
                .HasMany(t => t.Users)
                .WithOne(u => u.Tenant)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TenantTable>()
                .HasMany(t => t.Models)
                .WithOne(m => m.Tenant)
                .HasForeignKey(m => m.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TenantTable>()
                .HasMany(t => t.Schedulers)
                .WithOne(s => s.Tenant)
                .HasForeignKey(s => s.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
            /////////////////////////////////////////////////////////////////////////////////
            // Model Table Relationships
            // Model - User Realtionship (many - one)
            modelBuilder.Entity<ModelTable>()
                .HasOne(m => m.User)
                .WithMany(u => u.Models)
                .OnDelete(DeleteBehavior.Restrict);

            // Model - Tenant Relationship (many - one)
            modelBuilder.Entity<ModelTable>()
                .HasOne(m => m.Tenant)
                .WithMany(t => t.Models)
                .OnDelete(DeleteBehavior.Restrict);

            // Model - Datasource Relationship (many many)
            modelBuilder.Entity<DataSourceTable>().HasMany(m => m.Models);
            /////////////////////////////////////////////////////////////////////////////////
            // Scheduler Table Relationships

            // Scheduler - Tenant Relationship (many - one) 
            modelBuilder.Entity<SchedulerTable>()
                .HasOne(s => s.Tenant)
                .WithMany(t => t.Schedulers)
                .OnDelete(DeleteBehavior.Restrict);

            /////////////////////////////////////////////////////////////////////////////////
            //Data Source Table Relationships

            // DataSouce - User Relationship (many - one)
            modelBuilder.Entity<DataSourceTable>()
                .HasOne(ds => ds.User)
                .WithMany(u => u.DataSources)
                .OnDelete(DeleteBehavior.Restrict);                       

            // DataSource - Model Relationship (many many)
            modelBuilder.Entity<ModelTable>().HasMany(ds => ds.DataSources);
            /////////////////////////////////////////////////////////////////////////////////
        }
    }
}
