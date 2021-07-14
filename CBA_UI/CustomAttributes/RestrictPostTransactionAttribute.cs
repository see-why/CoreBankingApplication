using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBA.CustomAttributes
{
    // Used to enusre that Posting trasaction cant be carried out when the Business is closed
    public class RestrictPostTransactionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!MvcApplication.IsBusinessOpen)
            {
                HttpContext.Current.Session["UserAccessInfo"] = "You cannot carry out Posting transaction when business is closed";
                filterContext.Result = new RedirectResult("~/Home/Index");                
            }
        }
    }
}