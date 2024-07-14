using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netlernapi.Entity;

public class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id",TypeName = "uuid")]
    public Guid  Id { get; set; } = Guid.NewGuid();
    
    [Column("is_Active",TypeName = "boolean")]
    public bool IsActive { get; set; }=  true; 
    

    [Column("createdAt",TypeName = "timestamp with time zone")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    

    [Column("updatedAt",TypeName = "timestamp with time zone")]
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    
   
    
}