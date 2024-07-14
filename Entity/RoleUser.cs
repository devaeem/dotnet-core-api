using System.ComponentModel;

namespace netlernapi.Entity
{
    public enum RoleUser
    {  
        [Description("admin")]
        Admin,

        [Description("user")]
        User,

        [Description("guest")]
        Guest,

        [Description("superadmin")]
        Superadmin
    
    } 
    
}

