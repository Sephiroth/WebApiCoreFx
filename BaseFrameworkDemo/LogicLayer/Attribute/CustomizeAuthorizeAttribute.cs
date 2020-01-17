using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class CustomizeAuthorizeAttribute : AuthorizeAttribute
    {
        public string Permission { get; set; }

        public CustomizeAuthorizeAttribute() { }

        public CustomizeAuthorizeAttribute(string permission)
        {
            Permission = permission;
        }
    }
}