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

        public EFSystemContext(DbContextOptions<EFSystemContext> options) : base(options) { }

    }
}
