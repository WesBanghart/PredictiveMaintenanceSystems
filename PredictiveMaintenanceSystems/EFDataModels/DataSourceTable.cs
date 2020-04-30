using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

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

        //A static data file from a DataSource
        public byte[] File { get; set; }

        public string FileContentType { get; set; }
        public string FileName { get; set; }
        public long FileLength { get; set; }
        public string FileContentDisposition { get; set; }


        //Boolean determining if the DataSource is streaming or not.
        public bool IsStreaming { get; set; }

        //The datetime when the datasource entry was created
        public DateTime Created { get; set; }
        //The datetime when the datasource entry was last updated
        public DateTime? LastUpdated { get; set; }

        //Timestamp value, used as a concurrency token, value is automatically generated on insert/update
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public Guid UserId { get; set; }
        public UserTable User { get; set; }
    }
}
