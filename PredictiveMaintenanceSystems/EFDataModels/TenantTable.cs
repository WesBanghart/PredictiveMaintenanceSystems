using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFDataModels
{
    [Table("tenant_tbl")]
    public class TenantTable
    {
        //Primary key for tentant entries
        [Key]
        public Guid TenantId { get; set; }
        //Name of the company the tenant represents
        public string Company { get; set; }
        //Name of the primary contact for the tenant
        public string ContactName { get; set; }
        //Phone number of the primary contact for the tenant
        public string ContactPhone { get; set; }
        //Email address for the primary contact for the tenant
        public string ContactEmail { get; set; }

        //Timestamp value, used as a concurrency token, value is automatically generated on insert/update
        [Timestamp]
        public byte[] Timestamp { get; set; }

        //Represents a one-to-many tenant/users relationship
        public ICollection<UserTable> Users { get; set; }

    }
}
