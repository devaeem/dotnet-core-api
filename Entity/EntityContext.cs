using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace netlernapi.Entity
{
    
    public class EntityContext: DbContext
    {
        
        public EntityContext(DbContextOptions<EntityContext> options ) : base(options)
        {
            
        }
        
       
        public  virtual  DbSet<Category> Categories  { get; set; }
        public   virtual   DbSet<Product> Products  { get; set; }
        
        
      
    }
    
}

