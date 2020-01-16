using DBModel.Entity;
using DBModel.WxModel;
using IDBLayer.Interface;
using IdentityModel;
using LogicLayer.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApiCoreFx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IRepository<TbUser> rep;

        public LoginController(IRepository<TbUser> rep)
        {
            this.rep = rep;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(string name, string pwd)
        {
            TbUser user = await rep.GetEntityAsync(s => s.Name.Equals(name));
            if (user == null)
                return NotFound($"用户名\"{name}\"不存在");

            if (!user.Pwd.Equals(WxAppEncryptUtil.MD5(pwd)))
                return ValidationProblem(new ValidationProblemDetails() { Detail = "密码错误" });

            string token = AuthorizationUtil.GetToken(30, user.Id, user.Name, user.Phone, user.CarNum);
            DateTime authTime = DateTime.Now;
            DateTime expiresAt = authTime.AddMinutes(30);
            return Ok(new
            {
                access_token = token,
                token_type = "Jwt",
                profile = new
                {
                    sid = user.Id,
                    name = user.Name,
                    auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                    expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
                }
            });
        }

        public async Task<TbUser> WxLoginAsync(WxLoginParam loginParam)
        {
            TbUser user = null;
            OpenIdParam openIdParam = await WxUtils.GetOpenid(loginParam);
            if (openIdParam != null && !string.IsNullOrEmpty(openIdParam.session_key))
            {
                WxPhoneModel wxPhoneModel = WxAppEncryptUtil.GetEncryptedDataStr(loginParam.EncryptedData, openIdParam.session_key, loginParam.Iv);
                if (wxPhoneModel != null)
                {
                    string phone = wxPhoneModel.PurePhoneNumber ?? wxPhoneModel.PhoneNumber;
                    // Todo : 根据手机号处理用户信息
                }
            }
            return user;
        }
    }
}