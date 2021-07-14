using CBA.Core;
using CBA.Core.ViewModels;
using CBA.CustomAttributes;
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
    public class LoanAccountController : Controller
    {
        private LoanAccountLogic loanAccountLogic = new LoanAccountLogic();
        private CustomerAccountLogic customerAccountLogic = new CustomerAccountLogic();
        private AccountTypeDAO accountTypeDAO = new AccountTypeDAO();
        private LoanStatusDAO loanStatusDAO = new LoanStatusDAO();
        BranchLogic branchLogic = new BranchLogic();
      
        List<AccountType> accountTypes = new List<AccountType>();
        public LoanAccountController()
        {
            accountTypes.Add(new AccountType { Name = AccountTypeEnum.SAVINGS });
            accountTypes.Add(new AccountType { Name = AccountTypeEnum.CURRENT });
        }

        public ActionResult Index()
        {
            var allGLAccounts = loanAccountLogic.GetAll().OrderBy(b => b.Name);
            return View(allGLAccounts);
        }

        //
        // GET: /Branch/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
       

        //
        // GET: /Branch/Create
        [RestrictPostTransaction]
        public ActionResult AddLoanAccount(int? linkedCustomerAccountID)
        {
            if (linkedCustomerAccountID == 0 || linkedCustomerAccountID == null)
            {
                return RedirectToAction("ChooseCustomerAccount");
            }
            CustomerAccount customerAccount = customerAccountLogic.GetById(linkedCustomerAccountID.Value);
            if (customerAccount == null)
            {
                return RedirectToAction("ChooseCustomerAccount");
            }
            //ViewBag.Customer = customer;
            ViewBag.LinkedCustomerAccount = customerAccount;
            ViewBag.Customer = customerAccount.Customer;                      
            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");     
            return View();
        }

        //
        // POST: /Branch/Create
        [HttpPost]
        [RestrictPostTransaction]
        public ActionResult AddLoanAccount(AddLoanAccountView model)
        {
            CustomerAccount customerAccount = null;
            if (ModelState.IsValid)
            {
                if (model.LinkedCustomerAccountID == 0)
                {
                    return RedirectToAction("ChooseCustomerAccount");
                }
                customerAccount = customerAccountLogic.GetById(model.LinkedCustomerAccountID);
                if (customerAccount == null || customerAccount.IsActive == false)
                {
                    return RedirectToAction("ChooseCustomerAccount");
                }
                if (customerAccount.IsActive == false)
                {
                    return RedirectToAction("ChooseCustomerAccount");
                }
                
                if (model.RepaymentInterval > 60 || model.RepaymentInterval <= 0)
                {
                    ModelState.AddModelError("RepaymentInterval", "Invalid. Should not be greater than 60 days");
                }
                if (model.Duration < model.RepaymentInterval)
                {
                    ModelState.AddModelError("RepaymentInterval", "Should not be less than the duration");
                }
                //if (model.PrincipalAmount <=0 || model.InstallmentAmount <=0)
                //{
                //    ModelState.AddModelError("", "Amount has to be greater than zero");
                //}
                //if (model.PrincipalAmount < model.InstallmentAmount)
                //{
                //    ModelState.AddModelError("", "Installment can't be greater than the Principal Amount");
                //}
               
                Branch branch = branchLogic.GetById(model.BranchID);
                if (branch == null)
                {
                    ModelState.AddModelError("BranchID", "Branch does not exist");
                }                                   

                if (ModelState.IsValid)
                {
                    AccountType accountType = accountTypeDAO.GetByName(AccountTypeEnum.LOAN);
                    DateTime lastPaidDate = new FinancialDateLogic().GetById(1).CurrentFinancialDate;
                    double installmentAmount = model.PrincipalAmount / (model.Duration / model.RepaymentInterval);
                    LoanAccount newLoanAccount = new LoanAccount
                    {
                        Name = model.AccountName,
                        AccountNumber = loanAccountLogic.GenerateAccountNumber(accountType, customerAccount.Customer.CustomerID
                        , branch.ID),
                        PrincipalAmount = model.PrincipalAmount,                       
                        LinkedAccount = customerAccount,                        
                        RepaymentInterval = model.RepaymentInterval,
                        Duration = model.Duration,
                        LastPaidDate = lastPaidDate,
                        NextDueDate = lastPaidDate.AddDays(model.RepaymentInterval),
                        InstallmentAmount = installmentAmount,                        
                        Branch = branch,
                        Status = loanStatusDAO.GetById((int)LoanStatusEnum.UNPAID),
                        DueDate =  DateTime.Now.AddYears(1)                                             
                    };
                    //loanAccountLogic.Insert(newLoanAccount);
                    loanAccountLogic.ProcessNewLoan(newLoanAccount);
                    TempData["SuccessMessage"] = "Loan Account was created successfully";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.LinkedCustomerAccount = customerAccount;
            if (customerAccount != null)
                ViewBag.Customer = customerAccount.Customer;
            else
                ViewBag.Customer = null;            
            
            //ViewBag.CustomerAccount = customerAccount;             
            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
            return View(model);
        }

        //TODO:Test this with out an ID or a non integer
        // GET: /Branch/Edit/5
        public ActionResult EditLoanAccount(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("FindLoanAccount");
            }
            LoanAccount loanAccount = loanAccountLogic.GetById(id);
            if (loanAccount == null)
            {
                return RedirectToAction("FindLoanAccount");
            }

            ViewBag.LinkedCustomerAccount = loanAccount.LinkedAccount;
            ViewBag.Customer = loanAccount.LinkedAccount.Customer;
            ViewBag.AccountNumber = loanAccount.AccountNumber;
            ViewBag.PrincipalAmount = loanAccount.PrincipalAmount;

            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name",loanAccount.Branch.ID);
            //return View(new AddGLCategoryView { MainAccountCategoryID = 1 });
            return View(new EditLoanAccountView 
            {
                ID = loanAccount.ID,
                AccountName = loanAccount.Name,
                RepaymentInterval = loanAccount.RepaymentInterval,
                Duration = loanAccount.Duration,
                //InstallmentAmount = loanAccount.InstallmentAmount,
                BranchID = loanAccount.Branch.ID
            });

        }

        //TODO:Test this with out an ID or a non integer
        // POST: /Branch/Edit/5
        [HttpPost]
        public ActionResult EditLoanAccount(EditLoanAccountView model)
        {
            LoanAccount loanAccount = null;
            if (ModelState.IsValid)
            {
                if (model.ID == 0)
                {
                    return RedirectToAction("FindLoanAccount");
                }
                loanAccount = loanAccountLogic.GetById(model.ID);
                if (loanAccount == null)
                {
                    return RedirectToAction("FindLoanAccount");
                }
                if (model.RepaymentInterval > 60 || model.RepaymentInterval <= 0)
                {
                    ModelState.AddModelError("RepaymentInterval", "Invalid. Should not be greater than 60 days");
                }
                if (model.Duration < model.RepaymentInterval)
                {
                    ModelState.AddModelError("RepaymentInterval", "Should not be less than the duration");
                }
                //if (model.InstallmentAmount <= 0)
                //{
                //    ModelState.AddModelError("", "Amount has to be greater than zero");
                //}               
                Branch branch = branchLogic.GetById(model.BranchID);
                if (branch == null)
                {
                    ModelState.AddModelError("BranchID", "Branch does not exist");
                }                                   

                if (ModelState.IsValid)
                {
                    double installmentAmount = loanAccount.PrincipalAmount / (model.Duration / model.RepaymentInterval);
                    loanAccount.Name = model.AccountName;
                    loanAccount.RepaymentInterval = model.RepaymentInterval;
                    loanAccount.Duration = model.Duration;
                    loanAccount.InstallmentAmount = installmentAmount;
                    loanAccount.NextDueDate = loanAccount.LastPaidDate.AddDays(model.RepaymentInterval); 
                    loanAccount.Branch = branch;
                  
                    loanAccountLogic.Update(loanAccount);
                    TempData["SuccessMessage"] = "Loan Account was edited successfully";
                    return RedirectToAction("Index");
                }
            }
            if (loanAccount != null)
	        {
                ViewBag.LinkedCustomerAccount = loanAccount.LinkedAccount;
                ViewBag.Customer = loanAccount.LinkedAccount.Customer;
                ViewBag.AccountNumber = loanAccount.AccountNumber;
                ViewBag.PrincipalAmount = loanAccount.PrincipalAmount;
            }
            else
            {
                ViewBag.LinkedCustomerAccount = null;
                ViewBag.Customer = null;
                ViewBag.AccountNumber = "";
                ViewBag.PrincipalAmount = "";
            }                        

            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
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

        public ActionResult FindLoanAccount()
        {
            IList<LoanAccount> foundLoanAccounts = new List<LoanAccount>();
            ViewBag.foundLoanAccounts = foundLoanAccounts;
            return View();
        }
        [HttpPost]
        public ActionResult FindLoanAccount(FindLoanAccountView model)
        {
            IList<LoanAccount> foundLoanAccounts = new List<LoanAccount>();
            ViewBag.foundLoanAccounts = foundLoanAccounts;
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
                    foundLoanAccounts =loanAccountLogic.GetByAccountNumber(long.Parse(model.AccountNumber));
                }
                else if (!String.IsNullOrEmpty(model.AccountName))
                {
                    foundLoanAccounts = loanAccountLogic.GetByAccountName(model.AccountName);
                }
                if (foundLoanAccounts.Count == 0 && ModelState.IsValid)
                {
                    ModelState.AddModelError("", "No Loan account was found");
                }
            }

            //var allCustomers = customerLogic.GetAll().OrderBy(b => b.ID);
            ViewBag.foundLoanAccounts = foundLoanAccounts;
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
