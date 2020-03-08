using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFDataModels
{
    [Table("models_tbl")]
    class ModelTable
    {
        [Key]
        public string ModelId { get; set; }

        //[ForeignKey]
        public string UserId { get; set; }

        //[ForeignKey]
        public string TenantId { get; set; }

        public string ModelName { get; set; }

        public string Configuration { get; set; }

        public byte[] File { get; set; }

        public DateTime? LastUpdated { get; set; }

        public DateTime Created { get; set; }
    }
}
