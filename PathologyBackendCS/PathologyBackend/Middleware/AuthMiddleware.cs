using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PathologyBackend.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public AuthMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["token"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Token not found");
                return;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes("39fe38523879a281f185e862fdaf9aac6d0599a04d59dc2e052d0feef451ede1");

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = false,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var laboratoryId = jwtToken.Claims.First(x => x.Type == "laboratoryId").Value;

                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(ex.Message);
                return;
            }
        }

    }
}
