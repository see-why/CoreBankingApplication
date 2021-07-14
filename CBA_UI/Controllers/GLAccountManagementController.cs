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
    public class GLAccountController : Controller
    {
        private GLAccountLogic accountLogic = new GLAccountLogic();
        private GLCategoryLogic categoryLogic = new GLCategoryLogic();
        BranchLogic branchLogic = new BranchLogic();
        private MainAccountLogic mainAccountLogic = new MainAccountLogic();
        //
        // GET: /Branch/
        public ActionResult Index()
        {
            var allGLAccounts = accountLogic.GetAll().OrderBy(b => b.Name);
            ViewBag.foundGLAccounts = allGLAccounts;
            return View();
        }
        [HttpPost]
        public ActionResult Index(FindGLAccountView model)
        {
            //var allBranches = branchLogic.GetAll();
            IList<GLAccount> foundGLAccounts = new List<GLAccount>();

            if (ModelState.IsValid)
            {
                foundGLAccounts = accountLogic.FindGLAccountsByName(model.Name);
                if (foundGLAccounts.Count == 0)
                {
                    ModelState.AddModelError("", "No GL Account was found");
                }
            }
            ViewBag.foundGLAccounts = foundGLAccounts;
            return View(model);
        }

        //
        // GET: /Branch/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Branch/Create
        public ActionResult Create()
        {
            //ViewBag.AllGLCategories = new SelectList(categoryLogic.GetAll().OrderBy(b => b.Name),
            //    "ID", "Name");

            ViewBag.AllGLCategories = new SelectList(categoryLogic.GetAll().OrderBy(b => b.Name).Select(p => new 
                                        {
                                            Id = p.ID,
                                            Name = p.Name + " (" + p.MainCategory.Name + ")"
                                        }
                
            ),"Id","Name");
            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
            //return View(new AddGLCategoryView { MainAccountCategoryID = 1 });
            return View();
        }

        //
        // POST: /Branch/Create
        [HttpPost]
        public ActionResult Create(AddGLAccountView model)
        {
            if (ModelState.IsValid)
            {
                if (!accountLogic.IsGLAccountNameAvailable(model.Name))
                {
                    ModelState.AddModelError("Name", "A GL Account with this name alredy exists");
                }
                GLCategory category = categoryLogic.GetById(model.CategoryID);
                if (category == null)
                {
                    ModelState.AddModelError("CategoryID","Category does not exist");
                }
                Branch branch = branchLogic.GetById(model.BranchID);
                if (branch == null)
                {
                    ModelState.AddModelError("BranchID", "Branch does not exist");
                }                                   

                if (ModelState.IsValid)
                {
                    GLAccount newAccount = new GLAccount
                    {
                        Name = model.Name,
                        Code = accountLogic.GetCode(category),
                        GLCategory = category,
                        Branch = branch
                    };
                    accountLogic.Insert(newAccount);
                    TempData["SuccessMessage"] = "GL Account was created successfully";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.AllGLCategories = new SelectList(categoryLogic.GetAll().OrderBy(b => b.Name).Select(p => new
            {
                Id = p.ID,
                Name = p.Name + " (" + p.MainCategory.Name + ")"
            }

            ), "Id", "Name");
            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
            return View(model);
        }

        //TODO:Test this with out an ID or a non integer
        // GET: /Branch/Edit/5
        public ActionResult Edit(int id)
        {
            GLAccount account = accountLogic.GetById(id);
            if (account != null)
            {
                ViewBag.GLCategory = account.GLCategory.Name;
                ViewBag.GLCode = account.Code;
                ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
                return View(new EditGLAccountView
                {
                    Id = account.ID,
                    Name = account.Name,
                    BranchID = account.Branch.ID

                });
            }
            return RedirectToAction("Index");

        }

        //TODO:Test this with out an ID or a non integer
        // POST: /Branch/Edit/5
        [HttpPost]
        public ActionResult Edit(EditGLAccountView model)
        {

            if (ModelState.IsValid)
            {
                var account = accountLogic.GetById(model.Id);
                if (account != null)
                {
                    if (!accountLogic.IsGLAccountNameAvailable(model.Name) && (!account.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        ModelState.AddModelError("Name", "A GLCategory with this name alredy exists");
                    }
                    Branch branch = branchLogic.GetById(model.BranchID);
                    if (branch == null)
                    {
                        ModelState.AddModelError("BranchID", "Branch does not exist");
                    }
                    if (ModelState.IsValid)
                    {
                        account.Name = model.Name;
                        account.Branch = branch;
                        accountLogic.Update(account);
                        TempData["SuccessMessage"] = "GL Account was edited successfully";
                        return RedirectToAction("Index");
                    }
                    ViewBag.GLCategory = account.GLCategory.Name;
                    ViewBag.GLCode = account.Code;
                }

            }
            ViewBag.GLCategory = "";
            ViewBag.GLCode = "";
            return View(model);
        }

        //
        // GET: /Branch/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Branch/Delete/5
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
    }
}
