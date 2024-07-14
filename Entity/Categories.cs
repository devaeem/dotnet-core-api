using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netlernapi.Entity
{
    

    [Table("categories")]
    public class Category : BaseEntity
    {
        [MaxLength(255)]
        [Column("name",TypeName = "varchar(255)")]
        public string Name { get; set; } = string.Empty;
        
        
        public ICollection<Product>? Products { get; set;}
        
        
    }

}

