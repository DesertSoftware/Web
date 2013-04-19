//
//  Copyright 2013, Desert Software Solutions Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//

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
