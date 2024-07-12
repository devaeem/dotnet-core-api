using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netlernapi.Entity
{
    
    
    [Table("products")]
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id",TypeName = "int")]
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public int ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImage { get; set; }
        public int ProductStock { get; set; }
        public int ProductCategoryId { get; set; }
        public bool IsActive { get; set; }
        [Column("createdAt",TypeName = "timestamp")]
        public DateTime CreatedDate { get; set; }
        [Column("updatedAt",TypeName = "timestamp")]
        public DateTime UpdatedDate { get; set; }
    }

}



