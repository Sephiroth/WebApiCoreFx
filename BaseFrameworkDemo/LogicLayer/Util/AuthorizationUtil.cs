using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LogicLayer.Util
{
    /// <summary>
    /// 使用IdentityServer4的Jwt Token进行身份验证
    /// </summary>
    public class AuthorizationUtil
    {
        internal static SymmetricSecurityKey SymmetricKey { get; private set; }
        internal static SigningCredentials Credentials { get; private set; }

        static AuthorizationUtil()
        {
            SymmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("MicroHealth_LSH_KEY"));
            Credentials = new SigningCredentials(SymmetricKey, SecurityAlgorithms.HmacSha256Signature);
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="minutes">token有效时间(分钟)</param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="role"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string GetToken(int minutes, string id, string name, string role, string address)
        {
            if (string.IsNullOrEmpty(id)) { id = Guid.NewGuid().ToString(); }
            if (string.IsNullOrEmpty(name)) { name = Guid.NewGuid().ToString(); }
            if (string.IsNullOrEmpty(role)) { role = Guid.NewGuid().ToString(); }
            if (string.IsNullOrEmpty(address)) { address = Guid.NewGuid().ToString(); }
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            DateTime authTime = DateTime.UtcNow; // 生成时间
            DateTime expiresAt = authTime.AddMinutes(minutes); // 到期时间
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Audience, "api"),
                    new Claim(JwtClaimTypes.Issuer, "YFAPICommomCore"),
                    new Claim(JwtClaimTypes.Id, id),
                    new Claim(JwtClaimTypes.Name, name),
                    new Claim(JwtClaimTypes.Role, role),
                    new Claim(JwtClaimTypes.Address, address),
                }),
                Expires = expiresAt,
                SigningCredentials = Credentials
            };
            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// 验证Token是否有效
        /// </summary>
        /// <param name="token"></param>
        /// <param name="validTime">输出token剩余有效时间</param>
        /// <param name="claimsIdentity">输出身份信息</param>
        /// <returns></returns>
        public static bool VerifyToken(string token, out TimeSpan validTime, out ClaimsIdentity claimsIdentity)
        {
            validTime = default;
            claimsIdentity = null;
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            // Token字符串解析出来的JwtSecurityToken
            JwtSecurityToken readToken = tokenHandler.ReadJwtToken(token);
            TokenValidationParameters param = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudiences = readToken.Audiences,
                ValidateIssuer = true,
                ValidIssuer = readToken.Issuer,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SymmetricKey
            };
            // Token通过秘钥解析出来的SecurityToken
            SecurityToken sToken = null;
            bool rs = false;
            try
            {
                claimsIdentity = tokenHandler.ValidateToken(token, param, out sToken).Identity as ClaimsIdentity; //ClaimsPrincipal ce
                rs = sToken != null && claimsIdentity?.Actor != null;
            }
            catch { }
            if (rs)
            {   // 验证时间是否过期
                validTime = sToken.ValidTo - DateTime.Now;
                rs &= (validTime.TotalSeconds > 0);
            }
            return rs;
        }
    }
}
