using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Identity;

namespace netlernapi.Entity
{
    
    public class User: IdentityUser
    {


        [MaxLength(255)]
        [Column("firstName", TypeName = "varchar(255)")]
        public string FirstName { get; set; } = string.Empty;
        
        
        [MaxLength(255)]
        [Column("lastName",TypeName = "varchar(255)")]
        public string LastName { get; set; } = string.Empty;
        
        
        [MaxLength(255)]
        [Column("role",TypeName = "varchar(255)")]
        public string Role { get; set; } = RoleUser.User.ToString();
        
        
        // public BaseEntity BaseEntity { get; set; }
    }

 
}

