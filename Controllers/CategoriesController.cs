using DotnetApiApp.ModelsDto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netlernapi.Entity;

namespace netlernapi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]

    public class CategoriesController : ControllerBase
    {
        private EntityContext _context;
        
        public CategoriesController(EntityContext context)
        {
            _context = context;
        }
      
        [HttpGet]
        [Authorize]
        public async Task<IActionResult>  GetCategories(Pagination pagination)
  
        {
            try
            {
                var categories = await _context.Categories
                    .OrderBy(c => c.Name) 
                    .Select(
                        c => new
                        {
                            c.Id,
                            c.Name,
                            createdAt =  c.CreatedDate,
                            updatedAt=  c.UpdatedDate
                        }
                    )
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .AsNoTracking()
                    .ToListAsync();
            
                var total = await _context.Categories.CountAsync();
            
                var response = new
                {
                    mssage= "Fetch categories successfully",
                    rows = categories,
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
        public async Task<IActionResult> GetCategory(Guid id)
        {

            try
            {
                var category = await _context.Categories
                    .Where( c=> c.Id == id)
                    .Select( c => new
                    {
                        c.Id,
                        c.Name,
                        createdAt =  c.CreatedDate,
                        updatedAt=  c.UpdatedDate
                    })
                    .FirstOrDefaultAsync();
                if (category is null)
                {
                    return NotFound(new { msg = "Category not found" });
                }

                return Ok(category);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "Internal server error");
            }
            
            
            
           
        }



        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoriesDto categoriesDto)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var category = new Category
                {
                    Name = categoriesDto.Name,
                
                };
            
                await _context.Categories.AddAsync(category); 
                await _context.SaveChangesAsync(); 

                return Created("", new { msg = "Category created successfully" });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoriesDto categoriesDto)
        {

            try
            {
                var categoryExist = await _context.Categories.FindAsync(id);
    
                if (categoryExist == null)
                {
                    return NotFound(new { msg = "Id not found" });
                }
            
                categoryExist.Name = categoriesDto.Name;
                _context.Entry(categoryExist).State = EntityState.Modified;
                
                try
                {
                
                    await _context.SaveChangesAsync();
                
              
                }
                catch (DbUpdateException e)
                {
                    return BadRequest(new { message = e.Message });
                
                }
             
                
                return NoContent();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, $"Internal server error :{e.Message}");
            }
            
        }
       
        
        [HttpDelete("{id}")]
        public  async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category is null)
                {
                    return NotFound(new { msg = "Category not found" });
                }
            
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            
                return Ok(new { msg = $"Delete category successfully" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, $"Internal server error :{e.Message}");
            }
        }
    }
}