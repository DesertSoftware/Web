using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DesertSoftware.Web.Mvc
{
    public class RestrictToLocalHostAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <exception cref="System.Web.HttpRequestValidationException"></exception>
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (filterContext == null)
                throw new HttpRequestValidationException();

            if (!filterContext.HttpContext.Request.IsLocal)
                filterContext.Result = new HttpNotFoundResult("not found here");
        }
    }
}
