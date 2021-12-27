using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.WebPages.Razor.Configuration;

namespace GCBS_INTERNAL.Provider
{
    public class AllowCorsJsonAttribute : ActionFilterAttribute
    {
       

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
#if DEBUG
            if (filterContext.RequestContext.HttpContext.Request.IsLocal)
            {
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "*");
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS");
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            }
#endif
            base.OnActionExecuting(filterContext);
        }
    }
}