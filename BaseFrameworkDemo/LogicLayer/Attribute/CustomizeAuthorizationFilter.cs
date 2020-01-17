using LogicLayer.Util;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace LogicLayer.Attribute
{
    /// <summary>
    /// 自定义身份验证过滤器
    /// </summary>
    public class CustomizeAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            #region 根据反射获取自定义验证特性,判断是否需要验证
            bool caa = false;
            bool allowAnonymous = false;
            ControllerActionDescriptor cad = context.ActionDescriptor as ControllerActionDescriptor;
            string actionName = cad.ActionName;
            string controllerName = cad.ControllerName;
            foreach (var attr in cad.ControllerTypeInfo.CustomAttributes)
            {
                if (attr.AttributeType.Name.Equals("CustomizeAuthorizeAttribute"))
                {
                    caa = true;
                    break;
                }
            }
            if (!caa)
            {
                foreach (var attr in cad.MethodInfo.CustomAttributes)
                {
                    if (attr.AttributeType.Name.Equals("CustomizeAuthorizeAttribute"))
                    {
                        caa = true;
                        break;
                    }
                }
            }
            else
            {
                foreach (var attr in cad.MethodInfo.CustomAttributes)
                {
                    if (attr.AttributeType.Name.Equals("AllowAnonymousAttribute"))
                    {
                        allowAnonymous = true;
                        break;
                    }
                }
            }

            if ((caa && allowAnonymous) || !caa)
                return;
            #endregion

            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
                return;

            if (!(context.ActionDescriptor is ControllerActionDescriptor))
                return;

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
                        if (hadPower)
                            return;
                    }
                    context.Result = new ForbidResult();
                }
            }

            if (!rs || !vaildRs)
                context.Result = new UnauthorizedResult();

            return;
        }
    }
}