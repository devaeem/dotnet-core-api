using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DotnetApiApp.ModelsDto
{
    
   
    
    
    public class UpdateProductDto
    {
        
      
        [Display(Name ="โปรดระบุชื่อสินค้า")]
        [JsonProperty("name")]
        public string ProductName { get; set; } = string.Empty;
        
        
        [Display(Name ="โปรดระบุราคาสินค้า")]
        [JsonProperty("price")]
        public int ProductPrice { get; set; }
        
       
        [Display(Name ="โปรดระบุรายละเอียดสินค้า")]
        [JsonProperty("description")]
        public string ProductDescription { get; set; } = string.Empty;
        
        [Display(Name ="โปรดอัพรูปสินค้า")]
        [JsonProperty("images")]
        public string ProductImage { get; set; } = string.Empty;
        
        
        [Display(Name ="โปรดระบุชื่อหมวดหมู่")]
        [JsonProperty("category")]
        public Guid? Category { get; set; }
        
    }
    
    
}

