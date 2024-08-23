using WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _Configuration;
        private readonly IUserService _UserService;
        public UserController(IConfiguration Configuration, IUserService UserService)
        {
            _Configuration = Configuration;
            _UserService = UserService;
        }
        [AllowAnonymous]
        [HttpPost(nameof(Authentication))]
        public IActionResult Authentication([FromBody] LoginModel Data)
        {
            bool isValid = _UserService.IsValidUserInformation(Data);
            if (isValid)
            {
                var tokenString = GenerateJwtToken(Data.UserName);
                return Ok(new { Token = tokenString, Message = "Success" });
            }
            return BadRequest("Please pass the valid Username and Password");
        }
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(nameof(GetResult))]
        public IActionResult GetResult()
        {
            return Ok("API Validated");
        }
        /// <summary>
        /// Generate JWT Token after successful login.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private string GenerateJwtToken(string UserName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_Configuration["Jwt:key"] ?? "");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", UserName) }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _Configuration["Jwt:Issuer"],
                Audience = _Configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
