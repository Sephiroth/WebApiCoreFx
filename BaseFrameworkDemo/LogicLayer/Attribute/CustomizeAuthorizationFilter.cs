using IdentityModel;
using LogicLayer.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace LogicLayer.Attribute
{
    /// <summary>
    /// 自定义身份验证过滤器
    /// </summary>
    public class CustomizeAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor))
                return;

            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
                return;

            #region 根据反射获取自定义验证特性,判断是否需要验证
            ControllerActionDescriptor cad = context.ActionDescriptor as ControllerActionDescriptor;
            bool r1 = cad.ControllerTypeInfo.CustomAttributes.Any(s => s.AttributeType.Name.Equals("AuthorizeAttribute"));
            bool r2 = cad.MethodInfo.CustomAttributes.Any(s => s.AttributeType.Name.Equals("AuthorizeAttribute"));
            if (!r1 && !r2)
                return;

            string actionName = cad.ActionName;
            string controllerName = cad.ControllerName;
            #endregion

            bool rs = context.HttpContext.Request.Headers.TryGetValue("token", out Microsoft.Extensions.Primitives.StringValues strValues);
            bool vaildRs = false;
            if (rs)
            {
                vaildRs = AuthorizationUtil.VerifyToken(strValues.ToString(), out TimeSpan validTime, out ClaimsIdentity claimsIdentity);
                if (vaildRs)
                {
                    List<Claim> list = new List<Claim>(claimsIdentity.Claims);
                    Claim roleClaim = list.Find(s => s.Type.Contains(JwtClaimTypes.Role));
                    if (roleClaim != null)
                    {
                        //Todo:根据 actionName,controllerName,roleClaim判断是否有权限
                        bool hadPower = true;
                        if (!hadPower)
                        {
                            context.Result = new ForbidResult();
                            return;
                        }
                    }
                }
            }

            if (!rs || !vaildRs)
                context.Result = new UnauthorizedResult();

            return;
        }
    }
}