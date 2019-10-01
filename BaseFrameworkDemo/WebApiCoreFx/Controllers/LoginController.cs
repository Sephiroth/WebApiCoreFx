using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApiCoreFx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Authenticate()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                     new Claim(JwtClaimTypes.Audience,"api"),
                     new Claim(JwtClaimTypes.Issuer,"YFAPICommomCore"),
                     new Claim(JwtClaimTypes.Id, "1"),
                     new Claim(JwtClaimTypes.Name, "xxx"),
                     new Claim(JwtClaimTypes.Email, "xxx@qq.com"),
                     new Claim(JwtClaimTypes.PhoneNumber, "13500000000")
                }),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(Startup.symmetricKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new
            {
                access_token = tokenString,
                token_type = "Bearer",
                profile = new
                {
                    sid = "1",
                    name = "xxxx",
                    auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                    expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
                }
            });
        }
    }
}