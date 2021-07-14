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
    public class CustomerAccountController : Controller
    {
        private CustomerLogic customerLogic = new CustomerLogic();
        private CustomerAccountLogic customerAccountLogic = new CustomerAccountLogic();
        private AccountTypeDAO accountTypeDAO = new AccountTypeDAO();
        BranchLogic branchLogic = new BranchLogic();
        private MainAccountLogic mainAccountLogic = new MainAccountLogic();
        List<AccountType> accountTypes = new List<AccountType>();
        public CustomerAccountController()
        {
            accountTypes.Add(new AccountType { Name = AccountTypeEnum.SAVINGS });
            accountTypes.Add(new AccountType { Name = AccountTypeEnum.CURRENT });
        }

        public ActionResult Index()
        {
            var allGLAccounts = customerAccountLogic.GetAll().OrderBy(b => b.Name);
            return View(allGLAccounts);
        }

        //
        // GET: /Branch/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult ChooseCustomer()
        {
           
            IList<Customer> foundCustomers = new List<Customer>();
            ViewBag.foundCustomers = foundCustomers;
            return View();
        }
        [HttpPost]
        public ActionResult ChooseCustomer(FindCustomerView model)
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

        //
        // GET: /Branch/Create
        public ActionResult AddCustomerAccount(int ?customerID)
        {
            if (customerID == 0 || customerID == null)
            {
                return RedirectToAction("ChooseCustomer");
            }
            Customer customer = customerLogic.GetById(customerID.Value);
            if (customer == null)
            {
                return RedirectToAction("ChooseCustomer");
            }
            ViewBag.Customer = customer;
            
            //ViewBag.AllGLCategories = new SelectList(categoryLogic.GetAll().OrderBy(b => b.Name),
            //    "ID", "Name");

            ViewBag.AccountTypes = new SelectList(accountTypes.Select(p => new
            {
                Id = (int)p.Name,
                Name = p.Name.ToString()
           }), "Id", "Name");

            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
            //return View(new AddGLCategoryView { MainAccountCategoryID = 1 });
            return View();
        }

        //
        // POST: /Branch/Create
        [HttpPost]
        public ActionResult AddCustomerAccount(AddCustomerAccountView model)
        {
            Customer customer = null;
            if (ModelState.IsValid)
            {
                if (model.CustomerID == 0)
                {
                    return RedirectToAction("ChooseCustomer");
                }
                customer = customerLogic.GetById(model.CustomerID);
                if (customer == null)
                {
                    return RedirectToAction("ChooseCustomer");
                }
                AccountType accountType = null;
                AccountTypeEnum a = 0;
                if (!Enum.TryParse<AccountTypeEnum>(model.AccountType.ToString(), true, out a))
                    ModelState.AddModelError("AccountType", "Invalid Account Type");
                else
                {
                    accountType = accountTypeDAO.GetByName(a);
                } 
               
                Branch branch = branchLogic.GetById(model.BranchID);
                if (branch == null)
                {
                    ModelState.AddModelError("BranchID", "Branch does not exist");
                }                                   

                if (ModelState.IsValid)
                {
                    CustomerAccount newCustomerAccount = new CustomerAccount
                    {
                        Name = model.AccountName,
                        AccountNumber = customerAccountLogic.GenerateAccountNumber(accountType, customer.CustomerID, branch.ID),
                        //AccountNumber =1000001001,
                        Customer = customer,
                        Branch = branch,
                        AccountType = accountType,
                        IsActive = true,
                        LastInterestPaidDate = DateTime.Now
                     
                    };
                    customerAccountLogic.Insert(newCustomerAccount);
                    TempData["SuccessMessage"] = "Customer Account was created successfully";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Customer = customer;
             ViewBag.AccountTypes = new SelectList(accountTypes.Select(p => new
            {
                Id = (int)p.Name,
                Name = p.Name.ToString()
           }), "Id", "Name");

            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
            return View(model);
        }

        //TODO:Test this with out an ID or a non integer
        // GET: /Branch/Edit/5
        public ActionResult EditCustomerAccount(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("FindCustomerAccount");
            }
            CustomerAccount customerAccount = customerAccountLogic.GetById(id);
            if (customerAccount == null)
            {
                return RedirectToAction("FindCustomerAccount");
            }
            ViewBag.Customer = customerAccount.Customer;
            ViewBag.AccountNumber = customerAccount.AccountNumber;
            ViewBag.AccountType = customerAccount.AccountType.Name.ToString();

            ViewBag.AccountTypes = new SelectList(accountTypes.Select(p => new
            {
                Id = (int)p.Name,
                Name = p.Name.ToString()
            }), "Id", "Name",customerAccount.AccountType.ID);

            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name",customerAccount.Branch.ID);
            //return View(new AddGLCategoryView { MainAccountCategoryID = 1 });
            return View(new EditCustomerAccountView 
            {
                ID = customerAccount.ID,
                AccountName = customerAccount.Name,
                BranchID = customerAccount.Branch.ID
            });

        }

        //TODO:Test this with out an ID or a non integer
        // POST: /Branch/Edit/5
        [HttpPost]
        public ActionResult EditCustomerAccount(EditCustomerAccountView model)
        {
            CustomerAccount customerAccount = null;
            if (ModelState.IsValid)
            {
                if (model.ID == 0)
                {
                    return RedirectToAction("FindCustomerAccount");
                }
                customerAccount = customerAccountLogic.GetById(model.ID);
                if (customerAccount == null)
                {
                    return RedirectToAction("FindCustomerAccount");
                }                
               
                Branch branch = branchLogic.GetById(model.BranchID);
                if (branch == null)
                {
                    ModelState.AddModelError("BranchID", "Branch does not exist");
                }                                   

                if (ModelState.IsValid)
                {
                    customerAccount.Name = model.AccountName;
                    customerAccount.Branch = branch;
                  
                    customerAccountLogic.Update(customerAccount);
                    TempData["SuccessMessage"] = "Customer Account was edited successfully";
                    return RedirectToAction("Index");
                }
            }
            if (customerAccount != null)
	        {
                ViewBag.Customer = customerAccount.Customer ;
                ViewBag.AccountNumber = customerAccount.AccountNumber;
                ViewBag.AccountType = customerAccount.AccountType.Name.ToString();
            }
            else
            {
                ViewBag.Customer = null;
                ViewBag.AccountNumber = "";
                ViewBag.AccountType = "";
            }
            
             ViewBag.AccountTypes = new SelectList(accountTypes.Select(p => new
            {
                Id = (int)p.Name,
                Name = p.Name.ToString()
           }), "Id", "Name");

            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
            return View(model);
        }

        public ActionResult FindCustomerAccount()
        {
            IList<CustomerAccount> foundCustomerAccounts = new List<CustomerAccount>();
            ViewBag.foundCustomerAccounts = foundCustomerAccounts;
            return View();
        }
        [HttpPost]
        public ActionResult FindCustomerAccount(FindCustomerAccountView model)
        {
            IList<CustomerAccount> foundCustomerAccounts = new List<CustomerAccount>();
            ViewBag.foundCustomerAccounts = foundCustomerAccounts;
            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(model.AccountNumber) && String.IsNullOrWhiteSpace(model.AccountName))
                {
                    ModelState.AddModelError("", "You have to enter an Account Number or Account Name");
                }
                else if (!String.IsNullOrWhiteSpace(model.AccountNumber) && !ValidationLogic.IsValidAccountFormat(model.AccountNumber))
                {
                    ModelState.AddModelError("AccountNumber", "Invalid format. Enter a 11 digit Account Number");
                }

                else if (!String.IsNullOrWhiteSpace(model.AccountNumber))
                {
                    foundCustomerAccounts = customerAccountLogic.GetByAccountNumber(long.Parse(model.AccountNumber));
                }
                else if (!String.IsNullOrWhiteSpace(model.AccountName))
                {
                    foundCustomerAccounts = customerAccountLogic.GetByAccountName(model.AccountName);
                }
                if (foundCustomerAccounts.Count == 0 && ModelState.IsValid)
                {
                    ModelState.AddModelError("", "No customer account was found");
                }
            }

            //var allCustomers = customerLogic.GetAll().OrderBy(b => b.ID);
            ViewBag.foundCustomerAccounts = foundCustomerAccounts;
            return View(model);
        }

        public ActionResult CloseCustomerAccount(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("FindCustomerAccount");
            }
            CustomerAccount customerAccount = customerAccountLogic.GetById(id);
            if (customerAccount == null)
            {
                return RedirectToAction("FindCustomerAccount");
            }
            customerAccount.IsActive = false;
            customerAccountLogic.Update(customerAccount);
            TempData["SuccessMessage"] = "Customer Account Closed Successfully!!";
            return RedirectToAction("Index","Home");            

        }

        //TODO:Test this with out an ID or a non integer
        // POST: /Branch/Edit/5
        [HttpPost]
        public ActionResult CloseCustomerAccount(EditCustomerAccountView model)
        {
            CustomerAccount customerAccount = null;
            if (ModelState.IsValid)
            {
                if (model.ID == 0)
                {
                    return RedirectToAction("FindCustomerAccount");
                }
                customerAccount = customerAccountLogic.GetById(model.ID);
                if (customerAccount == null)
                {
                    return RedirectToAction("FindCustomerAccount");
                }

                Branch branch = branchLogic.GetById(model.BranchID);
                if (branch == null)
                {
                    ModelState.AddModelError("BranchID", "Branch does not exist");
                }

                if (ModelState.IsValid)
                {
                    customerAccount.Name = model.AccountName;
                    customerAccount.Branch = branch;

                    customerAccountLogic.Update(customerAccount);
                    return RedirectToAction("Index");
                }
            }
            if (customerAccount != null)
            {
                ViewBag.Customer = customerAccount.Customer;
                ViewBag.AccountNumber = customerAccount.AccountNumber;
                ViewBag.AccountType = customerAccount.AccountType.Name.ToString();
            }
            else
            {
                ViewBag.Customer = null;
                ViewBag.AccountNumber = "";
                ViewBag.AccountType = "";
            }

            ViewBag.AccountTypes = new SelectList(accountTypes.Select(p => new
            {
                Id = (int)p.Name,
                Name = p.Name.ToString()
            }), "Id", "Name");

            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
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
