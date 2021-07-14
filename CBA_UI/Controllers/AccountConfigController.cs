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
    public class AccountConfigController : Controller
    {   
        private SavingsConfigDAO savingsConfigDAO = new SavingsConfigDAO();
        private LoanConfigDAO loanConfigDAO = new LoanConfigDAO();
        private CurrentConfigDAO currentConfigDAO = new CurrentConfigDAO();
        private GLAccountLogic glAccountLogic = new GLAccountLogic();
              

        #region SAVINGS ACCOUNT
        public ActionResult SavingsConfigurationDetails() 
        {
            SavingsAccountConfiguration savingsConfig = savingsConfigDAO.GetById(1);            
            if (savingsConfig == null)          
                ViewBag.IsConfigured = false;
            else
                ViewBag.IsConfigured = true;

            return View(savingsConfig);
        }
        public ActionResult ConfigureSavings()
        {
            SavingsAccountConfiguration savingsConfig = savingsConfigDAO.GetById(1);
            ViewBag.InterestPayableAccounts = new SelectList(glAccountLogic.GetAllInterestExpenseAccount2().OrderBy(b => b.Name),
                "ID", "Name");
            if (savingsConfig != null)
            {
                //ViewBag.IsConfigured = false;                
                return View(new AddSavingsConfigView
                {
                    CreditInterestRate = savingsConfig.CreditInterestRate,
                    MinimumBalance = savingsConfig.MinimumBalance,
                    //InterestExpenseGLAccountID =
                });
            }
            return View();                       
        }

        [HttpPost]
        public ActionResult ConfigureSavings(AddSavingsConfigView model)
        {
            SavingsAccountConfiguration savingsConfig = null;
            if (ModelState.IsValid)
            {
                GLAccount interestPayableAccount = glAccountLogic.GetById(model.InterestExpenseGLAccountID);
                if (interestPayableAccount == null)                
                    ModelState.AddModelError("InterestExpenseGLAccountID","Invalid Interest Expense Acocunt");
                
                if (ModelState.IsValid) 
                {
                    savingsConfig = savingsConfigDAO.GetById(1);
                    if (savingsConfig == null)
                    {
                       savingsConfig = new SavingsAccountConfiguration
                       {
                           CreditInterestRate = model.CreditInterestRate,
                           MinimumBalance = model.MinimumBalance,
                           InterestExpenseGLAccount = interestPayableAccount,
                       };
                       savingsConfigDAO.InsertWithCommit(savingsConfig);
                    }
                    else
                    {
                        savingsConfig.CreditInterestRate = model.CreditInterestRate;
                        savingsConfig.MinimumBalance = model.MinimumBalance;
                        savingsConfig.InterestExpenseGLAccount = interestPayableAccount;

                        savingsConfigDAO.UpdateWithCommit(savingsConfig);
                    }
                    return RedirectToAction("SavingsConfigurationDetails");

                }
                
            }
            ViewBag.InterestPayableAccounts = new SelectList(glAccountLogic.GetAllInterestExpenseAccount2().OrderBy(b => b.Name),
                "ID", "Name"); ;
            return View(model);
        }
        #endregion

        #region CURRENT ACCOUNT
        public ActionResult CurrentConfigurationDetails()
        {
            CurrentAccountConfiguration CurrentAccountConfig = currentConfigDAO.GetById(1);
            if (CurrentAccountConfig == null)
                ViewBag.IsConfigured = false;
            else
                ViewBag.IsConfigured = true;

            return View(CurrentAccountConfig);
        }
        public ActionResult ConfigureCurrent()
        {
            CurrentAccountConfiguration CurrentAccountConfig = currentConfigDAO.GetById(1);
            ViewBag.InterestPayableAccounts = new SelectList(glAccountLogic.GetAllInterestExpenseAccount2().OrderBy(b => b.Name),
                "ID", "Name");
            ViewBag.COTIncomeGLAccounts = new SelectList(glAccountLogic.GetAllCOTIncomeGLAccounts2().OrderBy(b => b.Name),
                "ID", "Name");
            if (CurrentAccountConfig != null)
            {
                //ViewBag.IsConfigured = false;                
                return View(new AddCurrentConfigView
                {
                    CreditInterestRate = CurrentAccountConfig.CreditInterestRate,
                    MinimumBalance = CurrentAccountConfig.MinimumBalance,
                    InterestExpenseGLAccountID = CurrentAccountConfig.InterestExpenseGLAccount.ID,
                    COT = CurrentAccountConfig.COT,
                    COTIncomeGLAccountID = CurrentAccountConfig.COTIncomeGLAccount.ID,                    
                });
            }
            return View();
        }

        [HttpPost]
        public ActionResult ConfigureCurrent(AddCurrentConfigView model)
        {
            CurrentAccountConfiguration CurrentAccountConfig = null;
            if (ModelState.IsValid)
            {
                GLAccount interestPayableAccount = glAccountLogic.GetById(model.InterestExpenseGLAccountID);
                if (interestPayableAccount == null)
                    ModelState.AddModelError("InterestExpenseGLAccountID", "Invalid Interest Expense Acocunt");

                GLAccount cotIncomeGLAccount = glAccountLogic.GetById(model.COTIncomeGLAccountID);
                if (cotIncomeGLAccount == null)
                    ModelState.AddModelError("COTIncomeGLAccountID", "Invalid COT Income Acocunt");

                if (ModelState.IsValid)
                {
                    CurrentAccountConfig = currentConfigDAO.GetById(1);
                    if (CurrentAccountConfig == null)
                    {
                        CurrentAccountConfig = new CurrentAccountConfiguration
                        {
                            CreditInterestRate = model.CreditInterestRate,
                            MinimumBalance = model.MinimumBalance,
                            InterestExpenseGLAccount = interestPayableAccount,
                            COT = model.COT,
                            COTIncomeGLAccount = cotIncomeGLAccount,
                        };
                        currentConfigDAO.InsertWithCommit(CurrentAccountConfig);
                    }
                    else
                    {
                        CurrentAccountConfig.CreditInterestRate = model.CreditInterestRate;
                        CurrentAccountConfig.MinimumBalance = model.MinimumBalance;
                        CurrentAccountConfig.InterestExpenseGLAccount = interestPayableAccount;
                        CurrentAccountConfig.COT = model.COT;
                        CurrentAccountConfig.COTIncomeGLAccount = cotIncomeGLAccount;

                        currentConfigDAO.UpdateWithCommit(CurrentAccountConfig);
                    }
                    return RedirectToAction("CurrentConfigurationDetails");
                }

            }
            ViewBag.InterestPayableAccounts = new SelectList(glAccountLogic.GetAllInterestExpenseAccount2().OrderBy(b => b.Name),
                "ID", "Name"); ;
            ViewBag.COTIncomeGLAccounts = new SelectList(glAccountLogic.GetAllCOTIncomeGLAccounts2().OrderBy(b => b.Name),
                "ID", "Name");
            return View(model);
        }

        #endregion

        #region LOAN ACCOUNT
        public ActionResult LoanConfigurationDetails()
        {
            LoanAccountConfiguration loanConfig = loanConfigDAO.GetById(1);
            if (loanConfig == null)
                ViewBag.IsConfigured = false;
            else
                ViewBag.IsConfigured = true;

            return View(loanConfig);
        }
        public ActionResult ConfigureLoan()
        {
            LoanAccountConfiguration loanConfig = loanConfigDAO.GetById(1);
            ViewBag.InterestReceivableAccounts = new SelectList(glAccountLogic.GetAllInterestIncomeAccount2().OrderBy(b => b.Name),
                "ID", "Name");
            if (loanConfig != null)
            {
                return View(new AddLoanConfigView
                {
                    DebitInterestRate = loanConfig.DebitInterestRate,
                    InterestIncomeGLAccountID = loanConfig.InterestIncomeGLAccount.ID
                    
                });
            }
            return View();
        }

        [HttpPost]
        public ActionResult ConfigureLoan(AddLoanConfigView model)
        {
            LoanAccountConfiguration loanConfig = null;
            if (ModelState.IsValid)
            {
                GLAccount interestReceivableAccount = glAccountLogic.GetById(model.InterestIncomeGLAccountID);
                if (interestReceivableAccount == null)
                    ModelState.AddModelError("InterestIncomeGLAccountID", "Invalid Interest Income Acocunt");

                if (ModelState.IsValid)
                {
                    loanConfig = loanConfigDAO.GetById(1);
                    if (loanConfig == null)
                    {
                        loanConfig = new LoanAccountConfiguration
                        {
                            DebitInterestRate = model.DebitInterestRate,
                            InterestIncomeGLAccount = interestReceivableAccount,                            
                        };
                        loanConfigDAO.InsertWithCommit(loanConfig);
                    }
                    else
                    {
                        loanConfig.DebitInterestRate = model.DebitInterestRate;                        
                        loanConfig.InterestIncomeGLAccount = interestReceivableAccount;

                        loanConfigDAO.UpdateWithCommit(loanConfig);
                    }
                    return RedirectToAction("LoanConfigurationDetails");

                }

            }
            ViewBag.InterestReceivableAccounts = new SelectList(glAccountLogic.GetAllInterestExpenseAccount2().OrderBy(b => b.Name),
                "ID", "Name"); ;
            return View(model);
        }
        #endregion
    }
}
