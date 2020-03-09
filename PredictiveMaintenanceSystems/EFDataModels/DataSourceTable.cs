using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EFDataModels
{
    public class DataSourceTable
    {
        //Primary key for datasource entries
        [Key]
        public Guid DataSourceId { get; set; }
        //The name of the data source
        public string DataSourceName { get; set; }
        //The JSON configuration string of the data source
        public string Configuration { get; set; }
        //The connection string of the data source
        public string ConnectionString { get; set; }
        //The datetime when the datasource entry was created
        public DateTime Created { get; set; }
        //The datetime when the datasource entry was last updated
        public DateTime? LastUpdated { get; set; }

        //Timestamp value, used as a concurrency token, value is automatically generated on insert/update
        [Timestamp]
        public byte[] Timestamp { get; set; }

        //Represents of the many-to-many datasources/models relationship
        public ICollection<ModelTable> Models { get; set; }
        //Represents the relationship to a single user entity
        public Guid UserId { get; set; }
        public UserTable User { get; set; }
        //Represents the relationship to a single tenant entity
        public Guid TenantId { get; set; }
        public TenantTable Tenant { get; set; }
    }
}
