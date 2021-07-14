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
    public class CustomerController : Controller
    {
        private CustomerLogic customerLogic = new CustomerLogic();
        GenderDAO genderDAO = new GenderDAO();
        private GLCategoryLogic categoryLogic = new GLCategoryLogic();
        BranchLogic branchLogic = new BranchLogic();
        private MainAccountLogic mainAccountLogic = new MainAccountLogic();
        List<Gender> genders = new List<Gender>();
        public CustomerController()
        {
            genders.Add(new Gender{ Name = GenderEnum.MALE });
            genders.Add(new Gender{ Name = GenderEnum.FEMALE });
        }
        //
        // GET: /Branch/
        public ActionResult Index()
        {
            var allCustomers = customerLogic.GetAll().OrderBy(b => b.ID);
            return View(allCustomers);
        }
        //[HttpPost]
        //public ActionResult Index(SearchCustomerView model)
        //{
        //    IList<Customer> foundCustomers = new List<Customer>();
        //    if (ModelState.IsValid)
        //    {                
        //        if (model.CustomerID == 0 && String.IsNullOrEmpty(model.FirstName) && String.IsNullOrEmpty(model.LastName))
        //        {
        //            ModelState.AddModelError("", "You have to enter a search string");
        //        }
        //        else if (model.CustomerID != 0)
        //        {
        //            foundCustomers = customerLogic.GetByCustomerId(model.CustomerID);
        //        }
        //        else if (!String.IsNullOrEmpty(model.FirstName) && !String.IsNullOrEmpty(model.LastName))
        //        {
        //            foundCustomers = customerLogic.GetByFirstAndLastName(model.CustomerID);
        //        }
        //        else if (!String.IsNullOrEmpty(model.FirstName))
        //        {
        //            foundCustomers = customerLogic.GetByFirstName(model.CustomerID);
        //        }
        //        else if (!String.IsNullOrEmpty(model.LastName))
        //        {
        //            foundCustomers = customerLogic.GetByLastName(model.CustomerID);
        //        }
                
        //    }
        //    if (foundCustomers.Count == 0)           
        //    {
        //        ModelState.AddModelError("", "No customer was found");
        //    }
        //    //var allCustomers = customerLogic.GetAll().OrderBy(b => b.ID);
        //    return View(foundCustomers);
        //}

        public ActionResult FindCustomer()
        {
            IList<Customer> foundCustomers = new List<Customer>();
            ViewBag.foundCustomers = foundCustomers;
            return View();
        }
        [HttpPost]
        public ActionResult FindCustomer(FindCustomerView model) 
        {
            IList<Customer> foundCustomers = new List<Customer>();
            ViewBag.foundCustomers = foundCustomers;
            if (ModelState.IsValid)
            {

                if (String.IsNullOrWhiteSpace(model.FirstName) && String.IsNullOrWhiteSpace(model.LastName))
                {
                    ModelState.AddModelError("", "You have to enter a search string");
                }
                else if (!String.IsNullOrWhiteSpace(model.FirstName) && !String.IsNullOrWhiteSpace(model.LastName))
                {
                    foundCustomers = customerLogic.GetByFirstAndLastName(model.FirstName, model.LastName);
                }
                else if (!String.IsNullOrWhiteSpace(model.FirstName))
                {
                    foundCustomers = customerLogic.GetByFirstName(model.FirstName);
                }
                else if (!String.IsNullOrWhiteSpace(model.LastName))
                {
                    foundCustomers = customerLogic.GetByLastName(model.LastName);
                }
                if (foundCustomers.Count == 0 && ModelState.IsValid)
                {
                    ModelState.AddModelError("", "No customer was found");
                }
            }
            
             //var allCustomers = customerLogic.GetAll().OrderBy(b => b.ID);
             ViewBag.foundCustomers = foundCustomers;
             return View(model);
        }
        //[HttpPost]
        //public ActionResult SearchCustomer(SearchCustomerView model)
        //{
        //    IList<Customer> foundCustomers = new List<Customer>();
        //    ViewBag.foundCustomers = foundCustomers;
        //    if (ModelState.IsValid)
        //    {
        //        if (model.CustomerID == 0 && String.IsNullOrEmpty(model.FirstName) && String.IsNullOrEmpty(model.LastName))
        //        {
        //            ModelState.AddModelError("", "You have to enter a search string");
        //        }
        //        else if (model.CustomerID != 0)
        //        {
        //            foundCustomers = customerLogic.GetByCustomerId(model.CustomerID);
        //        }
        //        else if (!String.IsNullOrEmpty(model.FirstName) && !String.IsNullOrEmpty(model.LastName))
        //        {
        //            foundCustomers = customerLogic.GetByFirstAndLastName(model.FirstName, model.LastName);
        //        }
        //        else if (!String.IsNullOrEmpty(model.FirstName))
        //        {
        //            foundCustomers = customerLogic.GetByFirstName(model.FirstName);
        //        }
        //        else if (!String.IsNullOrEmpty(model.LastName))
        //        {
        //            foundCustomers = customerLogic.GetByLastName(model.LastName);
        //        }

        //    }
        //    if (foundCustomers.Count == 0)
        //    {
        //        ModelState.AddModelError("", "No customer was found");
        //    }
        //    //var allCustomers = customerLogic.GetAll().OrderBy(b => b.ID);
        //    ViewBag.foundCustomers = foundCustomers;
        //    return View(model);
        //}
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
            //ViewBag.Gender = new SelectList(Enum.GetValues(typeof(GenderEnum)));
           // ViewBag.Gender = new SelectList(Enum.GetValues(typeof(GenderEnum)));  
            ViewBag.Gender = new SelectList(genders.Select(p => new
            {
                Id = (int)p.Name,
                Name = p.Name.ToString()
            }), "Id", "Name");
            return View();
        }

        //
        // POST: /Branch/Create
        [HttpPost]
        public ActionResult Create(AddEditCustomerView model)
        {
            if (ModelState.IsValid)
            {

                Gender gender =null;
                //string gender = model.Gender.to
                GenderEnum g = 0;
                string email = model.Email.ToLower();
                if (customerLogic.IsCustomerExistingWithEmail(model.Email))
                {
                    ModelState.AddModelError("Email", "Sorry a customer exists with that Email");
                }
                if (!Enum.TryParse<GenderEnum>(model.Gender.ToString(), true, out g))
                    ModelState.AddModelError("Gender", "Invalid Gender");
                else 
                {
                    gender = genderDAO.GetByName(g);
                }                   
                                         
                if (ModelState.IsValid)
                {
                    Customer newCustomer = new Customer
                    {
                        CustomerID = customerLogic.GenerateCustomerId(),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        OtherNames = model.OtherNames,
                        PhoneNumber = model.PhoneNumber,
                        Email = email,
                        Address = model.Address,
                        Gender = gender
                    };
                    customerLogic.Insert(newCustomer);
                    TempData["SuccessMessage"] = "Customer was created successfully";
                    return RedirectToAction("Index");
                }
            }
            //ViewBag.Gender = new SelectList(Enum.GetValues(typeof(GenderEnum)));
            //ViewBag.Gender = new SelectList(genders,"Name","Name"); 
            ViewBag.Gender = new SelectList(genders.Select(p => new
            {
                Id = (int)p.Name,
                Name = p.Name.ToString()
            }), "Id", "Name");
            return View(model);
        }

        //TODO:Test this with out an ID or a non integer
        // GET: /Branch/Edit/5
        public ActionResult Edit(int id)
        {
            if (id != 0)
            {
                var customer = customerLogic.GetById(id);
                //ViewBag.Gender = new SelectList(Enum.GetValues(typeof(GenderEnum)));
                ViewBag.Gender = new SelectList(genders.Select(p => new 
                {
                    Id= (int)p.Name,
                    Name= p.Name.ToString()
                }), "Id", "Name", customer.Gender.ID);

                //ViewBag.Gender = new SelectList(genders, "Name", "Name", customer.Gender.Name.ToString());

                //ViewBag.Gender = new SelectList(new List<SelectListItem>(){
                //                        new SelectListItem { Text = "MALE", Value = "1" },
                //                        new SelectListItem { Text = "FEMALE ", Value = "2" }}, "Value", "Text"
                //                        , customer.Gender.ID); 
                ViewBag.CustomerID = customer.CustomerID.ToString("D6");
                int gender = customer.Gender.ID;
                return View(new AddEditCustomerView 
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    OtherNames = customer.OtherNames,
                    PhoneNumber = customer.PhoneNumber,
                    Email = customer.Email,
                    Address = customer.Address,
                    Gender = customer.Gender.ID
                });
            }
            TempData["SuccessMessage"] = "Customer was edited successfully";
            return RedirectToAction("Index");

        }

        //TODO:Test this with out an ID or a non integer
        // POST: /Branch/Edit/5
        [HttpPost]
        public ActionResult Edit(AddEditCustomerView model)
        {
            if (ModelState.IsValid)
            {
                var customer = customerLogic.GetById(model.ID);
                if (customer == null)
                {
                   return RedirectToAction("Index");
                }
                string email = model.Email.ToLower();
                if (customerLogic.IsCustomerExistingWithEmail(model.Email) && (!customer.Email.Equals(email)))
                {
                    ModelState.AddModelError("Email", "Sorry a user exists with that Email");
                }
                Gender gender = null;
                //string gender = model.Gender.to
                GenderEnum g = 0;
                if (!Enum.TryParse<GenderEnum>(model.Gender.ToString(), true, out g))
                    ModelState.AddModelError("Gender", "Invalid Gender");
                else
                {
                    gender = genderDAO.GetByName(g);
                }

                if (ModelState.IsValid)
                {                                                               
                    customer.FirstName = model.FirstName;
                    customer.LastName = model.LastName;
                    customer.OtherNames = model.OtherNames;
                    customer.PhoneNumber = model.PhoneNumber;
                    customer.Email = email;
                    customer.Address = model.Address;
                    customer.Gender = gender;
                    
                    customerLogic.Update(customer);
                    return RedirectToAction("Index");
                }
                ViewBag.Gender = new SelectList(genders.Select(p => new
                {
                    Id = (int)p.Name,
                    Name = p.Name.ToString()
                }), "Id", "Name", customer.Gender.ID);
            }
            //ViewBag.Gender = new SelectList(Enum.GetValues(typeof(GenderEnum)));
            //ViewBag.Gender = new SelectList(genders, "Name", "Name",model.Gender); 
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
