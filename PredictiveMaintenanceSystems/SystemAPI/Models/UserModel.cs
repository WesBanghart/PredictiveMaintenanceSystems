using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemAPI.Models
{
    [Table("user_tbl")]
    public class UserModel
    { 
        public UserModel(string userId, string tenantId, string userName, string email, bool isAdmin, string firstName, string lastName)
        {
            UserId = userId;
            TenantId = tenantId;
            UserName = userName;
            Email = email;
            IsAdmin = isAdmin;
            FirstName = firstName;
            LastName = lastName;
            Created = DateTime.Now;
            Models = null;
            DataSources = null;
        }
        [Key]
        public string UserId { get; set; }
        public string TenantId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DataSources { get; set; }
        public string Models { get; set; }
        public DateTime? Created { get; set; }

    }
}
