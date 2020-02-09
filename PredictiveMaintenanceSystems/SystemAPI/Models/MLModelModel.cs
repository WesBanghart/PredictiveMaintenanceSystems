using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SystemAPI.Models
{
    public class MlModelModel
    {
        [Key]
        public string MlModelId { get; set; }
        public string UserId { get; set; }
        public string TenantId { get; set; }
        public string ModelName { get; set; }
        public string Configuration { get; set; }
        public byte[] File { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
