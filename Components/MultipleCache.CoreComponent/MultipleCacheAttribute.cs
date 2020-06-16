using Microsoft.AspNetCore.Mvc.Filters;

namespace MultipleCache.CoreComponent.Redis
{
    public class MultipleCacheAttribute : ActionFilterAttribute
    {
        public MultipleCacheAttribute() { }
    }
}