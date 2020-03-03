using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SystemAPI.Models
{
    [Table("tenant_tbl")]
    public class TenantModel
    {
        [Key]
        public string TenantId { get; set; }
        public string CompanyName { get; set; }
    }
}
