using Microsoft.AspNetCore.Mvc;

namespace netlernapi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }
    
    
    }
    
    
}

