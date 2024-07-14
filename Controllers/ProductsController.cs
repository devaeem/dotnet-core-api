
using DotnetApiApp.ModelsDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netlernapi.Entity;

namespace netlernapi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly  EntityContext _context;
        
        public ProductsController(EntityContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(Pagination pagination)
        {

            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .OrderBy(p => p.ProductName)
                
                    .Select(p => new
                    {
                        p.Id,
                        name = p.ProductName,
                        is_active= p.IsActive,
                        price = p.ProductPrice, 
                        description = p.ProductDescription,
                        image = p.ProductImage,
                        category = p.Category != null ? new
                        {
                            p.Category.Id,
                            name = p.Category.Name,
                            is_active= p.IsActive,
                            createdAt =  p.CreatedDate,
                            updatedAt=  p.UpdatedDate,
                        }: null,
                        createdAt =  p.CreatedDate,
                        updatedAt=  p.UpdatedDate,
                    })
                    .Where( p => p.is_active == true)
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .AsNoTracking()
                    .ToListAsync();
            
                var total = await _context.Products.CountAsync();

                var response = new
                {
                    mssage = "Fetch categories successfully",
                    rows = products,
                    total,
                    hasNextPage = total > pagination.Page * pagination.PageSize,
                    pagination.Page,
                    pagination.PageSize
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "Internal server error");
            }
    
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {

            try
            {
                var products = await _context.Products
                    .Where( p=> p.Id == id)
                    .Select( p => new
                    {
                        p.Id,
                        name = p.ProductName,
                        is_active= p.IsActive,
                        price = p.ProductPrice, 
                        description = p.ProductDescription,
                        image = p.ProductImage,
                        category = p.Category != null ? new
                        {
                            p.Category.Id,
                            name = p.Category.Name,
                            is_active= p.IsActive,
                            createdAt =  p.CreatedDate,
                            updatedAt=  p.UpdatedDate,
                        }: null,
                        createdAt =  p.CreatedDate,
                        updatedAt=  p.UpdatedDate,
                    })
                    .FirstOrDefaultAsync();
                return products is null ? NotFound(new { msg = "Products not found" }) : Ok(products);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "Internal server error");
            }
           

           
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var category = await _context.Categories.FindAsync(productDto.Category);
                if (category == null)
                {
                    return BadRequest(new { message = "หมวดหมู่ที่ระบุไม่ถูกต้อง" });
                }

                var product = new Product
                {
                    ProductName = productDto.ProductName,
                    ProductPrice = productDto.ProductPrice,
                    ProductDescription = productDto.ProductDescription,
                    ProductImage = productDto.ProductImage, 
                    Category = category,
                };
                await _context.Products.AddAsync(product);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    return BadRequest(new { message = e.Message });
                }
            
                return Created("", new { msg = "Product created successfully" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "Internal server error");
            }
          
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductDto updateProductDto)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct is null)
                {
                    return NotFound(new { msg = "Product not found" });
                }
                if (!string.IsNullOrEmpty(updateProductDto.ProductName))
                {
                    existingProduct.ProductName = updateProductDto.ProductName;
                }

// อัพเดทราคาสินค้า
                if (updateProductDto.ProductPrice != 0)
                {
                    existingProduct.ProductPrice = updateProductDto.ProductPrice;
                }

// อัพเดทรายละเอียดสินค้า
                if (!string.IsNullOrEmpty(updateProductDto.ProductDescription))
                {
                    existingProduct.ProductDescription = updateProductDto.ProductDescription;
                }

// อัพเดทรูปภาพสินค้า
                if (!string.IsNullOrEmpty(updateProductDto.ProductImage))
                {
                    existingProduct.ProductImage = updateProductDto.ProductImage;
                }

// อัพเดทหมวดหมู่
                if (updateProductDto.Category.HasValue)
                {
                    var category = await _context.Categories.FindAsync(updateProductDto.Category.Value);
                    if (category is null)
                    {
                        return NotFound(new { msg = "ไม่พบหมวดหมู่" });
                    }
                    existingProduct.Category = category;
                }
               
                
                _context.Entry(existingProduct).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { msg = "Product updated successfully" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "Internal server error");
            }
          
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product is null)
                {
                    return NotFound(new { msg = "Product not found" });
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return Ok(new { msg = "Product deleted successfully" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "Internal server error");
            }
           
        }


    }

}