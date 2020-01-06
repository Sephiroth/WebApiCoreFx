using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UserCenterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("Login")]
        public string Login(string username, string pwd)
        {
            bool rs = false;
            // todo: 验证通过
            rs = true;
            string token = string.Empty;
            if (rs)
                token = GetToken("aaaa", "lutao", "13811112222", "111@qq.com");
            else
                throw new Exception("验证错误");
            return token;
        }

        private string GetToken(string id, string name, string phone, string email)
        {
            if (string.IsNullOrEmpty(id)) { id = Guid.NewGuid().ToString(); }
            if (string.IsNullOrEmpty(name)) { name = Guid.NewGuid().ToString(); }
            if (string.IsNullOrEmpty(phone)) { phone = Guid.NewGuid().ToString(); }
            if (string.IsNullOrEmpty(email)) { email = Guid.NewGuid().ToString(); }

            var tokenHandler = new JwtSecurityTokenHandler();
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddHours(1); // 有效时间
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(JwtClaimTypes.Audience, "api"),
                        new Claim(JwtClaimTypes.Issuer, "YFAPICommomCore"),
                        new Claim(JwtClaimTypes.Id, id),
                        new Claim(JwtClaimTypes.Name, name),
                        new Claim(JwtClaimTypes.Email, email),
                        new Claim(JwtClaimTypes.PhoneNumber, phone),
                        new Claim(JwtClaimTypes.Role,"Administrator")
                }),
                Expires = expiresAt, // Token有效时间
                SigningCredentials = new SigningCredentials(Startup.symmetricKey, SecurityAlgorithms.HmacSha256Signature),
            };
            tokenDescriptor.IssuedAt = authTime;

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}