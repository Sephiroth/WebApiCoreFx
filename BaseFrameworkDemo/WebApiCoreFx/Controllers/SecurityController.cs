using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiCoreFx.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly IAntiforgery antiforgery;

        public SecurityController(IAntiforgery antiforgery)
        {
            this.antiforgery = antiforgery;
        }

        [HttpGet]
        public async Task<IActionResult> GetXsrfToken()
        {
            var tokens = antiforgery.GetAndStoreTokens(HttpContext);
            Response.Cookies.Append("XSRF-TOKEN",
                tokens.RequestToken,
                new CookieOptions
                {
                    HttpOnly = false,
                    Path = "/",
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax
                }
            );
            await Response.WriteAsync(tokens.RequestToken);
            // 给需要验证的action添加ValidateAntiForgeryTokenAttribute
            return Ok(tokens.RequestToken);
        }

        [HttpGet]
        public Task<string> GetRSAPublicKey()
        {
            string s1 = EncryptionTool.OpenSsl.RSACryptoService.RSAInstance.Encrypt("你好");
            string s2 = EncryptionTool.OpenSsl.RSACryptoService.RSAInstance.Decrypt(s1);
            return Task.FromResult(EncryptionTool.OpenSsl.RSACryptoService.RSAInstance.RsaPublicKey);
        }
    }
}