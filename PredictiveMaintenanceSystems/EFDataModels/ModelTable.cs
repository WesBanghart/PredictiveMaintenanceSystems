using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFDataModels
{
    [Table("models_tbl")]
    public class ModelTable
    {
        //Primary key for model entries
        [Key]
        public Guid ModelId { get; set; }
        //The name of the model
        public string ModelName { get; set; }
        //The JSON configuration string of the model
        public string Configuration { get; set; }
        //The saved zipfile of the model
        public byte[] File { get; set; }
        //The datetime when the model entry was created
        public DateTime Created { get; set; }
        //The datetime when the model entry was last updated
        public DateTime? LastUpdated { get; set; }

        //Timestamp value, used as a concurrency token, value is automatically generated on insert/update
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public Guid UserId { get; set; }
        public UserTable User { get; set; }
    }
}
