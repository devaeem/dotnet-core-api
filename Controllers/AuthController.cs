using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotnetApiApp.ModelsDto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using netlernapi.Entity;

namespace netlernapi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<User> userManager,SignInManager<User> signInManager,
            IWebHostEnvironment webHostEnvironment,IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }
        
        

      [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto model)
{
    try
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return NotFound(new { msg = "ไม่พบผู้ใช้งานในระบบ" });

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

        if (!result.Succeeded)
        {
            return Unauthorized(new { msg = "Password ไม่ถูกต้อง" });
        }

        return await CreateToken(user.Email!);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return StatusCode(500, "Internal server error");
    }
}

private async Task<IActionResult> CreateToken(string email)
{
    try
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return NotFound();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName)
            // สามารถเพิ่ม ClaimTypes.Role หรือ claim อื่นๆ ตามต้องการได้
        };

        var jwtKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha256)
            //SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha256Signature)
      
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        var response = new
        {
            accessToken = tokenHandler.WriteToken(token),
            expires = token.ValidTo,
            fullname =$"{user.FirstName} {user.LastName}",
            role = user.Role 
        };

        return Ok(response);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return StatusCode(500, "Internal server error");
    }
}

[Authorize]
[HttpGet("profile")]
public async Task<IActionResult> Profile()
{
    try
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);

        if (user == null)
        {
            return NotFound(new { msg = "ไม่พบผู้ใช้งานในระบบ" });
        }

        return Ok(new
        {
            id = user.Id,
            fullname = user.FirstName + " " + user.LastName,
            role = user.Role 
        });
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return StatusCode(500, "Internal server error");
    }
}

        
       
        
        
        
        
    [HttpPost("register")]
        public async Task<IActionResult>  Register([FromBody] RegisterDto model)
        {
            try
            {
                
                var  user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)  return Conflict("อีเมลล์นี้มีผู้ใช้งานแล้ว");

                var Users = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,

                };
                
                
                var result = await _userManager.CreateAsync(Users, model.Password);
                
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                
                
              
                return Created("", new { message = "สมัครสมาชิกสำเร็จ"});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "Internal server error");
            }
            
            
        }
       
    
    }
    
}

