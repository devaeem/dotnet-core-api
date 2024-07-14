using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using netlernapi.Entity;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<EntityContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
builder.Services.AddDbContext<DotnetAPIAppIdentityDbContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured"))),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception is SecurityTokenExpiredException)
            {
                context.Response.Headers.Append("Token-Expired", "true");
            }
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var response = new
            {
                error = "Unauthorized",
                message = "คุณไม่มีสิทธิ์เข้าถึงทรัพยากรนี้"
            };
            return context.Response.WriteAsJsonAsync(response);
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";
            var response = new
            {
                error = "Forbidden",
                message = "คุณไม่มีสิทธิ์เข้าถึงทรัพยากรนี้"
            };
            return context.Response.WriteAsJsonAsync(response);
        }
    };
});


// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(
//
//         options =>
//         {
//
//
//             options.TokenValidationParameters = new TokenValidationParameters
//             {
//                 ValidateIssuer = true,
//                 ValidateAudience = true,
//                 ValidateLifetime = true,
//                 ValidateIssuerSigningKey = true,
//                 ValidIssuer = builder.Configuration["Jwt:Issuer"],
//                 ValidAudience = builder.Configuration["Jwt:Audience"],
//                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
//                 ClockSkew = TimeSpan.Zero
//             };
//
//             options.Events ??= new JwtBearerEvents();
//
//             options.Events.OnAuthenticationFailed = context =>
//             {
//                 if (context.Exception is SecurityTokenExpiredException)
//                 {
//                     context.Response.Headers.Add("Token-Expired", "true");
//                 }
//
//                 return Task.CompletedTask;
//             };
//
//             options.Events.OnChallenge = context =>
//             {
//                 if (context.AuthenticateFailure != null)
//                 {
//                     context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
//                     context.Response.ContentType = "application/json";
//
//                     var message = new
//                     {
//                         error = "Unauthorized",
//                         message = "You are not authorized to access this resource."
//                     };
//
//                     var json = JsonConvert.SerializeObject(message);
//                     return context.Response.WriteAsync(json);
//                 }
//
//                 return Task.CompletedTask;
//
//             };
//         }
//
//
//     );

builder.Services.Configure<IdentityOptions>(
    o =>
    {   
        o.Password.RequiredLength = 6;
        o.Password.RequireLowercase = false;
        o.Password.RequireUppercase = false;
        o.Password.RequireNonAlphanumeric = false;
        o.Password.RequireDigit = false;
    }
    
    );

builder.Services.AddCors();


builder.Services.AddDefaultIdentity<User>(
    options => options.SignIn.RequireConfirmedAccount = false
).AddEntityFrameworkStores<DotnetAPIAppIdentityDbContext>();

builder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.MaxDepth = 64;
    }
    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

 builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();

app.UseCors(options => {
    
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});

app.UseAuthorization();




app.MapControllers();

app.Run();
