using CBA.Core;
using CBA.Logic;
using CBA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBA.Controllers
{
    public class UserManagementController : Controller
    {
        public UserManagementController()
        {
            
        }
        BranchLogic branchLogic = new BranchLogic();
        UserLogic userLogic = new UserLogic();
        //
        // GET: /UserManagement/
        public ActionResult Index()
        {
            var allUsers = userLogic.GetAll();
            return View(allUsers);
            
        }

        //
        // GET: /UserManagement/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /UserManagement/Create
        public ActionResult Create()
        {
            
            //ViewBag.AllBranches = branchLogic.GetAll()
            //                                .OrderBy(b => b.Name)
            //                                .AsEnumerable()
            //                                .Select(b => new SelectListItem
            //                                {
            //                                    Text = b.Name,
            //                                    Value = b.ID.ToString(),                                               
            //                                });
            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
          

            return View();
        }

        //
        // POST: /UserManagement/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            ViewResult view;
            User user = new User();
            int branchID = int.Parse(collection["Branch.ID"]);
            //if (!TryUpdateModel(user))
            //{
            //    view = View();
            //}
            if (TryUpdateModel(user))
            {
                user.Email = user.Email.ToLower();
                user.UserName = user.UserName.ToLower();

                if (userLogic.IsUserExistingWithEmail(user.Email))
                {
                    ModelState.AddModelError("Email", "Sorry a user exists with that Email");
                }
                if (!userLogic.IsUserNameAvailable(user.UserName))
                {
                    ModelState.AddModelError("UserName", "Sorry that user name alredy exists ");
                }

                if (ModelState.IsValid)
                {
                    //int branchID = int.Parse(collection["Branch"]);
                    var branch = branchLogic.GetById(branchID);
                    user.Branch = branch;
                    //user.Password = userLogic.EncryptPassword(user.Password);
                    userLogic.Insert(user);
                    return RedirectToAction("Index");
                }
                //view = View(collection);
            }            
            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name",branchID);
            return View(user);
                     
        }

        //
        // GET: /UserManagement/Edit/5
        public ActionResult Edit(int id)
        {
            User user = userLogic.GetById(id);
            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
            return View(user);
        }

        //
        // POST: /UserManagement/Edit/5
        [HttpPost]

        public ActionResult Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
	        {
		 
	        }
            int userID = int.Parse(Request.Form["ID"]); //int.Parse(collection["ID"]);
            User user = userLogic.GetById(userID);
            int branchID = model.BranchID;
            string email = model.Email;
            if (userLogic.IsUserExistingWithEmail(user.Email) && (!user.Email.Equals(email)))
            {
                ModelState.AddModelError("Email", "Sorry a user exists with that Email");
            }           
            //string userName = collection["UserName"].ToLower();
            //if (!user.UserName.Equals(userName))
            //{
            //    return View(user);//someone tried to post another username
            //}
            //UpdateModel(user, null, null, new string[] { "ConfirmPassword" });            
            if (TryUpdateModel(user))
            {
                user.Email = user.Email.ToLower();
                user.UserName = user.UserName.ToLower();                              

                if (ModelState.IsValid)
                {
                    //int branchID = int.Parse(collection["Branch"]);
                    var branch = branchLogic.GetById(branchID);
                    user.Branch = branch;
                    //user.Password = userLogic.EncryptPassword(user.Password);
                    userLogic.Insert(user);
                    return RedirectToAction("Index");
                }
                //view = View(collection);
            }
            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name", branchID);
            return View(user);
        }

        //
        // GET: /UserManagement/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /UserManagement/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult ChangePassword(int id)
        {
            if (id == 0)
                return RedirectToAction("Index");

            User user = userLogic.GetById(id);
            if (user == null)
                return RedirectToAction("Index");
	                                               
            return View(user);
        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection collection)
        {
            int id = int.Parse(collection["ID"]);
            if (id == 0)
                return RedirectToAction("Index");
            User user = userLogic.GetById(id);
            if (user == null)
                return RedirectToAction("Index");

            string currentPassword = collection["CurrentPassword"];
            if (String.IsNullOrEmpty(currentPassword))
            {
                ModelState.AddModelError("CurrentPassword", "The Current Password is required");
                
            }
            else
	        {
                //if (!userLogic.IsPasswordValid(currentPassword, user.Password))
                //    ModelState.AddModelError("CurrentPassword","The Current Password is incorrect");
	        }
            
            if (ModelState.IsValid)
            {
                //user.Password = userLogic.EncryptPassword(collection["Password"]);
                userLogic.Insert(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

    }
}
