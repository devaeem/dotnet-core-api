using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DotnetApiApp.ModelsDto
{
    public class LoginDto
    {
    
        [Required (ErrorMessage = "โปรดระบุอีเมลล์ ")]
        [EmailAddress(ErrorMessage = "โปรดระบุอีเมลล์")]
        [Display(Name ="โปรดระบุอีเมลล์")]
        [JsonProperty("email")]
        public string Email { get; set; }= string.Empty;
        
        
        [Required (ErrorMessage = "โปรดระบุรหัสผ่าน")]
        [StringLength(250, ErrorMessage = "รหัสผ่านต้องมีความยาวอย่างน้อย 6 ตัวอักษร", MinimumLength = 6)]
        [JsonProperty("pasword")]
        public string Password { get; set; }= string.Empty;
        
        
    }
    
}

