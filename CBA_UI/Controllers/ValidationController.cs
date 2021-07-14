using CBA.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace CBA.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ValidationController : Controller 
    {
        //ValidationLogic valLogic = new ValidationLogic();
        UserLogic userLogic = new UserLogic();

                    
        public JsonResult IsUserNameAvailable(string username)
        {
            
            if (userLogic.IsUserNameAvailable(username))
                return Json(true, JsonRequestBehavior.AllowGet);
            string message = "Sorry that user name alredy exists";

            //string suggestedUID = String.Format(CultureInfo.InvariantCulture,
            //    "{0} is not available.", username);

            //for (int i = 1; i < 100; i++)
            //{
            //    string altCandidate = username + i.ToString();
            //    if (!_repository.UserExists(altCandidate))
            //    {
            //        suggestedUID = String.Format(CultureInfo.InvariantCulture,
            //       "{0} is not available. Try {1}.", username, altCandidate);
            //        break;
            //    }
            //}
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsBranchNameAvailable(string Name)
        {
            BranchLogic branchLogic = new BranchLogic();

            if (branchLogic.IsBranchNameAvailable(Name))
                return Json(true, JsonRequestBehavior.AllowGet);
            string message = "Sorry that branch name alredy exists";
           
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsUserExistingWithEmail(string Email)
        {
            if (!userLogic.IsUserExistingWithEmail(Email))
                return Json(true, JsonRequestBehavior.AllowGet);
            string message = "Sorry there is already a user with that Email";

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Validation/
        public ActionResult Index()
        {
            return View();
        }
	}
}