using CBA.Core;
using CBA.Core.ViewModels;
using CBA.Data;
using CBA.Logic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace CBA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TellerController : Controller
    {
        private TellerLogic tellerLogic = new TellerLogic();
        private UserLogic userLogic = new UserLogic();
        private GLAccountLogic glAccountLogic = new GLAccountLogic();
        private GLCategoryLogic categoryLogic = new GLCategoryLogic();
        BranchLogic branchLogic = new BranchLogic();
        private MainAccountLogic mainAccountLogic = new MainAccountLogic();
        //
        // GET: /Branch/
        public ActionResult Index()
        {
            var allTellers = tellerLogic.GetAll().OrderBy(t => t.User.UserName);
            ViewBag.foundTellers = allTellers;
            return View();
        }
        [HttpPost]
        public ActionResult Index(FindTellerView model)
        {
            IList<Teller> foundTellers = new List<Teller>();

            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(model.UserName) && String.IsNullOrWhiteSpace(model.TillAccountName))
                {
                    ModelState.AddModelError("", "You have to enter a search string");
                }
                else if (!String.IsNullOrWhiteSpace(model.UserName))
                {
                    foundTellers = tellerLogic.FindTellersByUsername(model.UserName);
                }
                else if (!String.IsNullOrWhiteSpace(model.TillAccountName))
                {
                    foundTellers = tellerLogic.FindTellersByTillAccountName(model.TillAccountName);
                }
                if (foundTellers.Count == 0 && ModelState.IsValid)
                {
                    ModelState.AddModelError("", "No Teller was found");
                }
                //foundTellers = tellerLogic.FindTellersBy(model.Name);
            }
            ViewBag.foundTellers = foundTellers;
            return View(model);
        }
        public ActionResult CreateTeller()
        {
            ViewBag.AllUnAssignedUsers = new SelectList(tellerLogic.GetUnAssignedUsers().OrderBy(b => b.UserName),
                "ID", "UserName");
            ViewBag.AllUnAssignedTills = new SelectList(tellerLogic.GetUnAssignedTills().OrderBy(b => b.Name),
                "ID", "Name");
            //ViewBag.AllUsers = new SelectList(userLogic.GetAll().OrderBy(b => b.UserName),
            //    "ID", "UserName");
            //ViewBag.AllTills = new SelectList(tellerLogic.GetAllTills().OrderBy(b => b.Name).Select(p => new
            //{
            //    Id = p.ID,
            //    Name = p.Name
            //}

            //), "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult CreateTeller(CreateTellerView model)
        {
            if (ModelState.IsValid)
            {

                GLAccount tillAccount = glAccountLogic.GetById(model.TillAccountID);
                if (tillAccount == null)
                {
                    ModelState.AddModelError("TillAccountID", "GL Account does not exist");
                }

                User user = userLogic.GetById(model.UserID);
                if (user == null)
                {
                    ModelState.AddModelError("UserID", "User does not exist");
                }
                if (ModelState.IsValid) 
                {
                    if (tellerLogic.IsUserAssigned(user))
                    {
                        ModelState.AddModelError("UserID", "The User is already assigned to a Till");
                    }
                    if (tellerLogic.IsTillAssigned(tillAccount))
                    {
                        ModelState.AddModelError("TillID", "The Till is already assigned to a User");
                    }
                    if (ModelState.IsValid)
                    {
                        Teller newTeller = new Teller
                        {
                            User = user,
                            TillAccount = tillAccount,                            
                        };
                        tellerLogic.Insert(newTeller);
                        TempData["SuccessMessage"] = "Teller was created successfully";
                        return RedirectToAction("Index");
                    }
                }
                
            }
            ViewBag.AllUsers = new SelectList(userLogic.GetAll().OrderBy(b => b.UserName),
                "ID", "UserName");
            //ViewBag.AllTills = new SelectList(tellerLogic.GetAllTills().OrderBy(b => b.Name).Select(p => new
            //{
            //    Id = p.ID,
            //    Name = p.Name 
            //}

            //), "Id", "Name");
            ViewBag.AllUnAssignedTills = new SelectList(tellerLogic.GetUnAssignedTills().OrderBy(b => b.Name),
                "ID", "Name");
              
            return View(model);
        }
    }
}
