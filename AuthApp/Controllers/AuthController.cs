using AuthApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public ILogger<AuthController> Logger { get; }
        public IConfiguration Configuration { get; }

        public AuthController(ILogger<AuthController> logger, IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            var tokenString = "";
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Model");
            }
            if(loginModel.UserName=="Akash" && loginModel.Password == "Akash")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "Issuer",
                    audience: "Audience",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(Configuration["JWT:Expiry"])),
                    signingCredentials: signinCredentials
                );
                tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);                
            }
            return Ok(new { Token = tokenString });
        }
    }
}
