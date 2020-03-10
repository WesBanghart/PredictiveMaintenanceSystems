using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFDataModels
{
    [Table("user_tbl")]
    public class UserTable
    {
        //Primary key for user entries
        [Key]
        public Guid UserId { get; set; }
        //Username used for system access
        public string UserName { get; set; }
        //Email address of the user
        public string Email { get; set; }
        //First name of the user
        public string FirstName { get; set; }
        //Last name of the user
        public string LastName { get; set; }
        //The datetime when the user entry was created
        public DateTime Created { get; set; }
        //The datetime when the user entry was last updated
        public DateTime? LastUpdate { get; set; }

        //Timestamp value, used as a concurrency token, value is automatically generated on insert/update
        [Timestamp]
        public byte[] Timestamp { get; set; }

        //Represents a one-to-many user/models relationship
        public ICollection<ModelTable> Models { get; set; }
        //Represents a one-to-many user/datasources relationship
        public ICollection<DataSourceTable> DataSources { get; set; }
        //Represents a one-to-many user/schedulers relationship
        public ICollection<SchedulerTable> Schedulers { get; set; }
        //Represents the relationship to a single tenant entity
        public Guid TenantId { get; set; }
        public TenantTable Tenant { get; set; }
    }
}
