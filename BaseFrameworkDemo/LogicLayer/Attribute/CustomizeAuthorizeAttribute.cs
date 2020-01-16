using LogicLayer.Util;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using IdentityModel;

namespace LogicLayer.Attribute
{
    public class CustomizeAuthorizeAttribute : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool rs = context.HttpContext.Request.Headers.TryGetValue("token", out Microsoft.Extensions.Primitives.StringValues strValues);
            if (rs)
            {
                rs = AuthorizationUtil.VerifyToken(strValues.ToString(), out TimeSpan validTime, out System.Security.Claims.ClaimsIdentity claimsIdentity);
                if (rs)
                {
                    List<Claim> list = new List<Claim>(claimsIdentity.Claims);
                    list.Find(s=>s.ValueType.Equals(JwtClaimTypes.Role));
                }
            }
        }
    }
}