using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace netlernapi.Entity
{
    
    
    public class DotnetAPIAppIdentityDbContext : IdentityDbContext<User>
    {
        public DotnetAPIAppIdentityDbContext(DbContextOptions<DotnetAPIAppIdentityDbContext> options) : base(options)
        {
        }
    }
    

}

