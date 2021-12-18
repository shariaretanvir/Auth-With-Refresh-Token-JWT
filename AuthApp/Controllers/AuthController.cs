using AuthApp.Model;
using AuthApp.Utilities;
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
        public IToken Token { get; }

        public AuthController(ILogger<AuthController> logger, IConfiguration configuration,IToken token)
        {
            Logger = logger;
            Configuration = configuration;
            Token = token;
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
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,loginModel.UserName),
                    new Claim(ClaimTypes.Role, "Manager")
                };

                tokenString = Token.GenerateToken(claims);

            }
            return Ok(new { Token = tokenString });
        }
    }
}
