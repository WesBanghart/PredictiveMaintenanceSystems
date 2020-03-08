using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFDataModels
{
    [Table("user_tbl")]
    class UserTable
    {
        [Key]
        public string UserId { get; set; }

        //[ForeignKey]
        public string TenantId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DataSources { get; set; }

        public string Models { get; set; }

        public DateTime Created { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}
