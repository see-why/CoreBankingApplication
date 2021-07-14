using CBA.Core;
using CBA.Logic;
using CBA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CBA
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           // SetCurrentBuisnessStatus();
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
            
        }
        public void SetCurrentBuisnessStatus()
        {
            EODLogic eodLogic = new EODLogic();
            EOD eod = eodLogic.GetById(1);

            IsBusinessOpen = eod.BusinessStatus == Business.OPEN;
            
        }
        public static bool IsBusinessOpen { get; set; }
    }
}
