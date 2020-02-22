using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SystemAPI.Models
{
    public class UserModel
    {
        [Key]
        public string UserId { get; set; }
        public string TenantId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public string Name { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
