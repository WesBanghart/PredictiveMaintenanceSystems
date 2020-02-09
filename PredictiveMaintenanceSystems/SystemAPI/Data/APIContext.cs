using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SystemAPI.Data
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SessionModel>(eb =>
                {
                    eb.HasNoKey();
                });
        }
        public DbSet<UserModel> UserModels { get; set; }
        public DbSet<SessionModel> SessionModels { get; set; }
        public DbSet<MlModelModel> MlModelModels { get; set; }
        public DbSet<TenantModel> TenantModels { get; set; }
    }
}
