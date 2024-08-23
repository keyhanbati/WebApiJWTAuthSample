using System;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Services;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Middleware
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly IConfiguration _Configuration;
        private readonly IUserService _UserService;

        public JWTMiddleware(RequestDelegate Next, IConfiguration Configuration, IUserService UserService)
        {
            _Next = Next;
            _Configuration = Configuration;
            _UserService = UserService;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachAccountToContext(context, token);

            await _Next(context);
        }

        private void attachAccountToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_Configuration["Jwt:Key"] ?? "");
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = jwtToken.Claims.First(x => x.Type == "id").Value;

                // attach account to context on successful jwt validation
                context.Items["User"] = _UserService.GetUserDetails();
            }
            catch
            {
                // do nothing if jwt validation fails
                // account is not attached to context so request won't have access to secure routes
            }
        }
    }
}
