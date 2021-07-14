using CBA.Core;
using CBA.Core.ViewModels;
using CBA.Data;
using CBA.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CBA.Controllers
{

    [Authorize(Roles = "Admin")]
    public class BranchController : Controller
    {
        private BranchLogic branchLogic= new BranchLogic();
        //
        // GET: /Branch/
        public ActionResult Index()
        {
            var allBranches = branchLogic.GetAll();
            ViewBag.foundBranches = allBranches;
            //return View(allBranches);
            return View();

        }
        [HttpPost]
        public ActionResult Index(FindBranchView model)
        {
            //var allBranches = branchLogic.GetAll();
            IList<Branch> foundBranches = new List<Branch>();
            
            if (ModelState.IsValid)
            {
                foundBranches = branchLogic.SearchBranch(model.Name);
                if (foundBranches.Count == 0)
                {
                    ModelState.AddModelError("", "No Branch was found");
                }
            }
            ViewBag.foundBranches = foundBranches;
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
            return View();
        }
        

        //
        // POST: /Branch/Create
        [HttpPost]
        public ActionResult Create(Branch branch)
        {
            if (!String.IsNullOrEmpty(branch.Name))
            {
                if (!branchLogic.IsBranchNameAvailable(branch.Name))
                {
                    ModelState.AddModelError("Name", "A branch with this name alredy exists");
                }
            }
            
            if (ModelState.IsValid)
            {
                branchLogic.Insert(branch);
                TempData["SuccessMessage"] = "Branch was created successfully";
                return RedirectToAction("Index");
            }
            ////return RedirectToAction("Index");
            //try
            //{
               
            //    // TODO: Add insert logic here

                
            //}
            //catch
            //{
            //    return View();
            //}
           
           return View(branch);
        }

        //
        // GET: /Branch/Edit/5
        public ActionResult Edit(int id)
        {
            Branch branch = branchLogic.GetById(id);
            //return View(branch);
            return View(new EditBranchView {
                ID = branch.ID,
                Name = branch.Name            
            });
        }

        //
        // POST: /Branch/Edit/5
        //[HttpPost]
        //public ActionResult Edit(FormCollection collection)
        //{
        //    int id = int.Parse(collection["ID"]);
        //    string name = collection["Name"];
        //    var branch = branchLogic.GetById(id); 
        //    //check if the branchName is available. And then we check if the user edited the Name field
        //    if (!String.IsNullOrEmpty(name) && branch != null)
        //    {
        //        if (!branchLogic.IsBranchNameAvailable(name) && (!branch.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        //        {
        //            ModelState.AddModelError("Name", "A branch with this name alredy exists");
        //        }
        //    }
            
        //    if (ModelState.IsValid)
        //    {
        //        UpdateModel(branch);                             
        //        branchLogic.Update(branch);
        //        TempData["SuccessMessage"] = "Branch was updated successfully";
        //        return RedirectToAction("Index");
        //    }
            
        //    return View();
        //}
        [HttpPost]
        public ActionResult Edit(EditBranchView model)
        {
            //int id = int.Parse(collection["ID"]);
            string name = model.Name;
            var branch = branchLogic.GetById(model.ID);
            //check if the branchName is available. And then we check if the user edited the Name field
            if (!String.IsNullOrEmpty(name) && branch != null)
            {
                if (!branchLogic.IsBranchNameAvailable(name) && (!branch.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("Name", "A branch with this name alredy exists");
                }
            }

            if (ModelState.IsValid)
            {
                UpdateModel(branch);
                branchLogic.Update(branch);
                TempData["SuccessMessage"] = "Branch was updated successfully";
                return RedirectToAction("Index");
            }

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
