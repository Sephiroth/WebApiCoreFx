using DBModel.Entity;
using IDBLayer.Interface;
using IdentityModel;
using LogicLayer.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using WxAppUtil.Model;
using WxAppUtil.Util;

namespace WebApiCoreFx.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IRepository<TbUser> rep;
        private readonly IHttpClientFactory clientFactory;

        public LoginController(IRepository<TbUser> rep, IHttpClientFactory clientFactory)
        {
            this.rep = rep;
            this.clientFactory = clientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(string name, string pwd)
        {
            TbUser user = await rep.GetEntityAsync(s => s.Name.Equals(name));
            if (user == null)
                return NotFound($"用户名'{name}'不存在");
            if (!WxAppEncryptUtil.MD5(pwd).Equals(user.Pwd))
                return ValidationProblem(new ValidationProblemDetails() { Detail = "密码错误" });

            string token = AuthorizationUtil.GetToken(30, user.Id, user.Name, "user", user.CarNum);
            DateTime authTime = DateTime.Now;
            DateTime expiresAt = authTime.AddMinutes(30);
            return Ok(new
            {
                access_token = token,
                token_type = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
                profile = new
                {
                    sid = user.Id,
                    name = user.Name,
                    auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                    expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
                }
            });
        }

        [HttpPost]
        public async Task<TbUser> WxLoginAsync(WxLoginParam loginParam)
        {
            TbUser user = null;
            string phone = null;
            // 使用IHttpClientFactory创建的HttpClient
            OpenIdParam openIdParam = await WxUtils.GetOpenIdAsync(loginParam, clientFactory.CreateClient());
            if (openIdParam != null && !string.IsNullOrEmpty(openIdParam.session_key))
            {
                WxPhoneModel wxPhoneModel = WxAppEncryptUtil.GetEncryptedDataStr(loginParam.EncryptedData, openIdParam.session_key, loginParam.Iv);
                if (wxPhoneModel != null)
                {
                    phone = wxPhoneModel.PurePhoneNumber ?? wxPhoneModel.PhoneNumber;
                }
            }
            return user;
        }

    }
}