using CBA.Core;
using CBA.Core.ViewModels;
using CBA.CustomAttributes;
using CBA.Data;
using CBA.Logic;
using CBA.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CBA.Controllers
{

   // [AllowAnonymous]
   // [Authorize(Roles = "TELLER")]
    
    public class TellerPostController : Controller
    {
        private CustomerAccountLogic customerAccountLogic = new CustomerAccountLogic();
        private TellerLogic tellerLogic = new TellerLogic();
        private FinancialDateDAO financialDateDAO = new FinancialDateDAO();
        private GLAccountLogic accountLogic = new GLAccountLogic();
        private GLCategoryLogic categoryLogic = new GLCategoryLogic();
        private TellerPostLogic tellerPostLogic = new TellerPostLogic();

        public UserManager<User> UserManager { get; private set; }
        public TellerPostController()
        {
            UserManager = new UserManager<User>(new UserStore<User>(new ApplicationDbContext()));
        }
        public User CurrentUser 
        {
            get 
            {
                //UserManager<User> userManager = new UserManager<User>(new UserStore<User>(new ApplicationDbContext()));
                return UserManager.FindById(User.Identity.GetUserId());
            }
        }
       // private MainAccountDAO mainAccountDAO = new MainAccountDAO();
        //
        [Authorize(Roles = "Admin")]
        // GET: /Branch/
        public ActionResult Index()
        {
            var allTellerPosts = tellerPostLogic.GetAll();
            return View(allTellerPosts);
        }
        public ActionResult ViewTellerPost()
        {
            var allTellerPosts = tellerPostLogic.GetAllTellerPosts(CurrentUser).OrderBy(x=> x.FinancialDate);
            return View(allTellerPosts);
        }

        //
        // GET: /Branch/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /GLPosting/PostTransaction
        [RestrictPostTransaction]
        public ActionResult PostTransaction(int? customerAccountID)
        {            
            if (!tellerLogic.IsUserAssigned(CurrentUser))
            {
                TempData["ErrorMessage"] = "Sorry you are not assigned to a Till Account";
                return RedirectToAction("Index","Home");
            }

            if (customerAccountID == 0 || customerAccountID == null)
            {
                return RedirectToAction("ChooseCustomerAccount");
            }
            CustomerAccount customerAccount = customerAccountLogic.GetById(customerAccountID.Value);
            if (customerAccount == null)
            {
                return RedirectToAction("ChooseCustomerAccount");
            }
            ViewBag.CustomerAccount = customerAccount;
            ViewBag.Customer = customerAccount.Customer;
            //ViewBag.PostTypes =
            return View();
        }

        //
        // POST: /Branch/Create
        [RestrictPostTransaction]
        [HttpPost]
        public ActionResult PostTransaction(PostCustomerAccountView model)
        {
            CustomerAccount customerAccount = null;
            if (ModelState.IsValid)
            {
                if (model.CustomerAccountID == 0)
                {
                    return RedirectToAction("ChooseCustomerAccount");
                }
                customerAccount = customerAccountLogic.GetById(model.CustomerAccountID);
                if (customerAccount == null)
                {
                    return RedirectToAction("ChooseCustomerAccount");
                }
                //if (model.Amount < 200 || model.Amount > 2000000)
                //{
                //    ModelState.AddModelError("", "Amount should not be less than 200 and greater than 2000000");
                //}
                PostingType postType = 0;

                if (!Enum.TryParse<PostingType>(model.TransactionType.ToString(), true, out postType))
                    ModelState.AddModelError("TransactionType", "Invalid Transaction Type");

                Teller teller = tellerLogic.GetTellerByUser(CurrentUser);
                if (postType == PostingType.WITHDRAWAL)
	            {
                    
                    if(!customerAccountLogic.IsDeductable(customerAccount,model.Amount))
                    {
                        ModelState.AddModelError("", "Transaction not Possible. Insufficient Balance!!");
                    }
                    else if (!tellerLogic.IsTillCreditable(teller, model.Amount))
                    {
                        ModelState.AddModelError("", "Transaction not Possible. Insufficient Balance in your Till!!");
                    }
		 
	            }                             
                
                if (ModelState.IsValid)
                {
                    //Teller teller = tellerLogic.GetTellerByUser(CurrentUser);
                    TellerPosting tellerPost = new TellerPosting 
                    {
                        CustomerAccount = customerAccount,
                        Teller = teller,
                        PostingType = postType,
                        Amount  = model.Amount,
                        Narration = model.Narration,
                        FinancialDate = financialDateDAO.GetById(1).CurrentFinancialDate
                    };
                    bool success;
                    if (postType == PostingType.DEPOSIT)
                    {
                        success = tellerPostLogic.ProcessDeposit(tellerPost, teller);
                    }
                    else
                    {
                        success = tellerPostLogic.ProcessWithDraw(tellerPost);
                        // = tellerPostLogic.ProcessWithDraw(tellerPost, teller);
                    }
                    if (!success)
                    {
                        TempData["ErrorMessage"] = "Sorry Transaction was not successful!!";
                        return RedirectToAction("Index","Home");
                    }
                    TempData["SuccessMessage"] = "Transaction was successful";
                    return RedirectToAction("ViewTellerPost");
                }
            }
            ViewBag.CustomerAccount = customerAccount;
            if (customerAccount != null)            
                ViewBag.Customer = customerAccount.Customer;                                 
            return View(model);
        }

        public ActionResult ChooseCustomerAccount()
        {
            IList<CustomerAccount> foundCustomerAccounts = new List<CustomerAccount>();
            ViewBag.foundCustomerAccounts = foundCustomerAccounts;
            return View();
        }
        [HttpPost]
        public ActionResult ChooseCustomerAccount(FindCustomerAccountView model)
        {
            IList<CustomerAccount> foundCustomerAccounts = new List<CustomerAccount>();
            ViewBag.foundCustomerAccounts = foundCustomerAccounts;
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(model.AccountNumber) && String.IsNullOrEmpty(model.AccountName))
                {
                    ModelState.AddModelError("", "You have to enter an Account Number or Account Name");
                }
                else if (!String.IsNullOrEmpty(model.AccountNumber) && !ValidationLogic.IsValidAccountFormat(model.AccountNumber))
                {
                    ModelState.AddModelError("AccountNumber", "Invalid format. Enter a 11 digit Account Number");
                }

                else if (!String.IsNullOrEmpty(model.AccountNumber))
                {
                    foundCustomerAccounts = customerAccountLogic.GetByAccountNumber(long.Parse(model.AccountNumber));
                }
                else if (!String.IsNullOrEmpty(model.AccountName))
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
      
        //
        // GET: /Branch/Delete/5
        //public ActionResult Delete(int id)  
        //{
        //    return View();
        //}

        //
        // POST: /Branch/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
