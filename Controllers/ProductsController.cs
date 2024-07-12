
using DotnetApiApp.ModelsDto;
using Microsoft.AspNetCore.Mvc;

namespace ControllerBasedApiSample.Controllers
{
 [Route("api/[Controller]")]
 [ApiController]
 public class ProductsController : ControllerBase
 {

  private static readonly Product[] Summaries = new[]
     {
    new Product { Id = 1, ProductName = "Notebook" ,ProductPrice = 35500,ProductCateGory = "ProductIT" },
    new Product { Id = 2, ProductName = "Laptop", ProductPrice = 25500,ProductCateGory = "ProductIT" },
    new Product { Id = 3, ProductName = "Tablet",ProductPrice = 34500,ProductCateGory = "ProductIT" }


    };

  [HttpGet]
  public IActionResult Index([FromQuery] Pagination pagination)
  {


   var res = new
   {
    msg = "Fetch Data Products Sucessfully",
    data = Summaries,
    pageProduct = pagination.Page,
    pageSizeProduct = pagination.PageSize

   }
;
   return Ok(res);

  }
  [HttpGet("{id}")]
  public IActionResult GeTById(int id)
  {
   return Ok(new { msg = $"Hello Users : {id} " });

  }



 }

 public class Product
 {
  public int Id { get; set; }
  public string? ProductName { get; set; }
  public int ProductPrice { get; set; }
  public string? ProductCateGory { get; set; }
 }


}