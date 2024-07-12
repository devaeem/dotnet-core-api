using Microsoft.EntityFrameworkCore;

namespace netlernapi.Entity
{
    
    public class EntityContext: DbContext
    {
        public EntityContext(DbContextOptions<EntityContext> options) : base(options)
        {
           
        }
        
       
        
        public DbSet<Products> Products  { get; set; }
        
      
    }
    
}

