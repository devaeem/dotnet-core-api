using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netlernapi.Entity
{
    
    [Table("products")]
    public class Product : BaseEntity
    {
        [MaxLength(255)]
        [Column("product_name",TypeName = "varchar(255)")]
        public string? ProductName { get; set; }
        [Column("product_price",TypeName = "int")]
        public int ProductPrice { get; set; }
        [MaxLength(255)]
        [Column("product_description",TypeName = "varchar(255)")]
        public string? ProductDescription { get; set; }
        [MaxLength(255)]
        [Column("product_image",TypeName = "varchar(255)")]
        public string? ProductImage { get; set; }

        [ForeignKey("Category_id")]
        public Category Category { get; set; } = null!;





    }

}



