using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DotnetApiApp.ModelsDto;

public class CategoriesDto
{
    [Required (ErrorMessage = "โปรดระบุชื่อหมวดหมู่")]
    [Display(Name ="โปรดระบุชื่อหมวดหมู่")]
    [JsonProperty("name")]
    public string  Name { get; set; } = string.Empty;

   
}