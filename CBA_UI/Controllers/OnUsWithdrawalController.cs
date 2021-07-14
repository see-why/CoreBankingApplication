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
    public class OnUsWithdrawalController : Controller
    {
        private ATMTerminalLogic atmLogic = new ATMTerminalLogic();
        private GLCategoryLogic categoryLogic = new GLCategoryLogic();
        //private ATMTerminalLogic atmLogic = new ATMTerminalLogic();
        //
        // GET: /Branch/
        public ActionResult Index()
        {
            var allTerminals = atmLogic.GetAll().OrderBy(b => b.Name);
            ViewBag.foundTerminals = allTerminals;
            return View();
        }
        [HttpPost]
        public ActionResult Index(ATMTerminal model)
        {
            //var allBranches = branchLogic.GetAll();
            IList<ATMTerminal> foundTerminals = new List<ATMTerminal>();

            if (ModelState.IsValid)
            {
                foundTerminals = atmLogic.FindTerminalByName(model.Name);
                if (foundTerminals.Count == 0)
                {
                    ModelState.AddModelError("", "No ATM Terminal was found");
                }
            }
            ViewBag.foundTerminals = foundTerminals;
            return View(model);
        }

        
        public ActionResult AddATMTerminal()
        {           
            return View();
        }

        //
        // POST: /Branch/Create
        [HttpPost]
        public ActionResult AddATMTerminal(AddEditATMTerminalView model)
        {
            if (ModelState.IsValid)
            {
                if (!atmLogic.IsTerminalNameAvailable(model.Name))
                {
                    ModelState.AddModelError("Name", "A Terminal with this name alredy exists");
                }
                
                if (ModelState.IsValid)
                {
                    ATMTerminal newATMTerminal= new ATMTerminal
                    {
                        Name = model.Name,
                        TerminalID = model.TerminalID,
                        Location = model.Location,                       
                    };
                    atmLogic.Insert(newATMTerminal);
                    TempData["SuccessMessage"] = "ATM Terminal was added successfully";
                    return RedirectToAction("Index");
                }
            }
                                 
            return View(model);
        }

        //TODO:Test this with out an ID or a non integer
        // GET: /Branch/Edit/5
        public ActionResult EditATMTerminal(int id)
        {
            ATMTerminal terminal = atmLogic.GetById(id);
            if (terminal != null)
            {                                
                return View(new AddEditATMTerminalView
                {  
                    ID = terminal.ID,
                    Name = terminal.Name,
                    TerminalID = terminal.TerminalID,
                    Location = terminal.Location,
                });
            }
            return RedirectToAction("Index");

        }

        //TODO:Test this with out an ID or a non integer
        // POST: /Branch/Edit/5
        [HttpPost]
        public ActionResult EditATMTerminal(AddEditATMTerminalView model)
        {

            if (ModelState.IsValid)
            {
                var terminal = atmLogic.GetById(model.ID);
                if (terminal != null)
                {
                    if (!atmLogic.IsTerminalNameAvailable(model.Name) && (!terminal.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        ModelState.AddModelError("Name", "A GLCategory with this name alredy exists");
                    }
                                       
                    if (ModelState.IsValid)
                    {
                        terminal.Name = model.Name;
                        terminal.TerminalID = model.TerminalID;
                        terminal.Location = model.Location;

                        atmLogic.Update(terminal);
                        TempData["SuccessMessage"] = "ATM Terminal was edited successfully";
                        return RedirectToAction("Index");
                    }
                    //ViewBag.GLCategory = terminal.GLCategory.Name;
                    //ViewBag.GLCode = terminal.Code;
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
