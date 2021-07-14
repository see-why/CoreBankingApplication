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
    public class GLCategoryController : Controller
    {
        private GLCategoryLogic categoryLogic= new GLCategoryLogic();
        private MainAccountLogic mainAccountLogic = new MainAccountLogic();
        //
        // GET: /Branch/
        public ActionResult Index()
        {
            var allCategories = categoryLogic.GetAll();
            return View(allCategories);
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
            ViewBag.AllMainAccounts = new SelectList(mainAccountLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
            //return View(new AddGLCategoryView { MainAccountCategoryID = 1 });
            return View();
        }

        //
        // POST: /Branch/Create
        [HttpPost]
        public ActionResult Create(AddGLCategoryView model)
        {
            if (ModelState.IsValid)
            {
                if (!categoryLogic.IsGLCategoryNameAvailable(model.Name))
                {
                    ModelState.AddModelError("Name", "A Category with this name alredy exists");
                }

                MainAccount mainAccount = mainAccountLogic.GetById(model.MainAccountCategoryID);
                //it should never be null except the user tries to over post data
                if (mainAccount == null)
                {
                     ModelState.AddModelError("MainAccountCategoryID", "Branch does not exist");
                }
                
                if (ModelState.IsValid)
                {
                    GLCategory newCategory = new GLCategory
                    {
                        Name = model.Name,
                        MainCategory = mainAccount,
                        Description = model.Description

                    };
                    categoryLogic.Insert(newCategory);
                    TempData["SuccessMessage"] = "GL Category was created successfully!!";
                    return RedirectToAction("Index");
                }
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
            ViewBag.AllMainAccounts = new SelectList(mainAccountLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
           
           return View(model);
        }

        //TODO:Test this with out an ID or a non integer
        // GET: /Branch/Edit/5
        public ActionResult Edit(int id)
        {            
            GLCategory category = categoryLogic.GetById(id);
            if (category!=null)
            {
                ViewBag.MainAccountCategory = category.MainCategory.Name.ToString();
                return View(new EditGLCategoryView
                {
                    Id = category.ID,
                    Name = category.Name,
                    Description = category.Description

                });
            }
            return RedirectToAction("Index");
           
        }

        //TODO:Test this with out an ID or a non integer
        // POST: /Branch/Edit/5
        [HttpPost]
        public ActionResult Edit(EditGLCategoryView model)
        {
            
            if (ModelState.IsValid)
            {
                var category = categoryLogic.GetById(model.Id);
                if (category != null)
                {
                    if (!categoryLogic.IsGLCategoryNameAvailable(model.Name) && (!category.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        ModelState.AddModelError("Name", "A category with this name alredy exists");
                    }
                    if (ModelState.IsValid)
                    {
                        category.Name = model.Name;                        
                        category.Description = model.Description;
                        categoryLogic.Update(category);
                        TempData["SuccessMessage"] = " GL Category was edited successfully";
                        return RedirectToAction("Index");
                    }
                    ViewBag.MainAccountCategory = category.MainCategory.Name.ToString();
                }
                
            }
            ViewBag.MainAccountCategory = "";
           
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
