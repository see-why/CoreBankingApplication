using System.Web;
using System.Web.Mvc;

namespace CBA
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //var authorizeAttribute = new System.Web.Mvc.AuthorizeAttribute();
            //authorizeAttribute.Roles = "Admin";
            //filters.Add(authorizeAttribute);
            filters.Add(new System.Web.Mvc.AuthorizeAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
