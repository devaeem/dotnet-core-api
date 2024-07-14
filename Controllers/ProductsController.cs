
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
                    createdAt =  p.CreatedDate,
                    updatedAt=  p.UpdatedDate,
                    category = p.Category != null ? new
                    {
                        p.Category.Id,
                        name = p.Category.Name,
                        is_active= p.IsActive,
                        createdAt =  p.CreatedDate,
                        updatedAt=  p.UpdatedDate,
                    }: null
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
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var products = await _context.Products.FindAsync(id);
            return products is null ? NotFound(new { msg = "Products not found" }) : Ok(products);

            return Ok(products);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)
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
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct is null)
            {
                return NotFound(new { msg = "Product not found" });
            }

            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductPrice = product.ProductPrice;
            existingProduct.ProductDescription = product.ProductDescription;
            existingProduct.ProductImage = product.ProductImage;
            existingProduct.Category = product.Category;
            
            await _context.SaveChangesAsync();
            return Ok(existingProduct);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
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


    }

}