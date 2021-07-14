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
    public class EODController : Controller
    {
        private SavingsConfigDAO savingsConfigDAO = new SavingsConfigDAO();
        private LoanConfigDAO loanConfigDAO = new LoanConfigDAO();
        private CurrentConfigDAO currentConfigDAO = new CurrentConfigDAO();
        private FinancialReportLogic financialReportLogic = new FinancialReportLogic();
        private EODLogic eodLogic = new EODLogic();
        private FinancialDateLogic finDateLogic = new FinancialDateLogic();
        public ActionResult CloseBusiness(int? id)
        {
            EOD eod = eodLogic.GetById(1);            
            if (eod.BusinessStatus == Business.CLOSED)
            {
                ViewBag.IsBusinessClosed = true;
                ViewBag.BusinessClosedMessage = "Business is Closed already";
                
                //TempData["SuccessMessage"] = "Business Status is closed already!!";
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.IsBusinessClosed = false;
                SavingsAccountConfiguration savingsConfig = savingsConfigDAO.GetById(1);
                CurrentAccountConfiguration CurrentAccountConfig = currentConfigDAO.GetById(1);
                LoanAccountConfiguration loansConfig = loanConfigDAO.GetById(1);
                if (savingsConfig == null || CurrentAccountConfig == null || loansConfig == null)
                {
                    ViewBag.IsAllAccountsConfigured = false;
                    if (savingsConfig == null)
                        ViewBag.IsSavingsConfigured = false;
                    else
                        ViewBag.IsSavingsConfigured = true;
                    if (CurrentAccountConfig == null)
                        ViewBag.IsCurrentAccountConfigured = false;
                    else
                        ViewBag.IsCurrentAccountConfigured = true;
                    if (loansConfig == null)
                        ViewBag.IsLoanConfigured = false;
                    else
                        ViewBag.IsLoanConfigured = true;
                }
                else
                {
                    ViewBag.IsAllAccountsConfigured = true;
                }
            }
            
            //if (ViewBag.IsAllAccountsConfigured)
            //{
            //    eodLogic.RunEOD(savingsConfig, CurrentAccountConfig, loansConfig);
            //} 
            ViewBag.CurrentFinancialDate = finDateLogic.GetById(1).CurrentFinancialDate.ToString("dd, MMMM, yyyy");           
            return View();
        }

        [HttpPost]
        public ActionResult CloseBusiness() 
        {
            EOD eod = eodLogic.GetById(1);
            if (eod.BusinessStatus == Business.CLOSED)
            {
                ViewBag.IsBusinessClosed = true;
                ViewBag.BusinessClosedMessage = "Business is Closed already!!";
                //TempData["SuccessMessage"] = "Business Status is closed already!!";
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.IsBusinessClosed = false;
                SavingsAccountConfiguration savingsConfig = savingsConfigDAO.GetById(1);
                CurrentAccountConfiguration CurrentAccountConfig = currentConfigDAO.GetById(1);
                LoanAccountConfiguration loansConfig = loanConfigDAO.GetById(1);
                if (savingsConfig == null || CurrentAccountConfig == null || loansConfig == null)
                {
                    ViewBag.IsAllAccountsConfigured = false;
                    if (savingsConfig == null)
                        ViewBag.IsSavingsConfigured = false;
                    else
                        ViewBag.IsSavingsConfigured = true;
                    if (CurrentAccountConfig == null)
                        ViewBag.IsCurrentAccountConfigured = false;
                    else
                        ViewBag.IsCurrentAccountConfigured = true;
                    if (loansConfig == null)
                        ViewBag.IsLoanConfigured = false;
                    else
                        ViewBag.IsLoanConfigured = true;
                }
                else
                {
                    ViewBag.IsAllAccountsConfigured = true;
                }
                if (ViewBag.IsAllAccountsConfigured)
                {
                    eodLogic.RunEOD(savingsConfig, CurrentAccountConfig, loansConfig);
                    MvcApplication.IsBusinessOpen = false;
                    ViewBag.IsBusinessClosed = true;
                    ViewBag.BusinessClosedMessage = "Business was closed successfully";
                    //TempData["SuccessMessage"] = "Business was closed succesfully!!";
                    //return RedirectToAction("Index", "Home");
                }
            }            
            
            ViewBag.CurrentFinancialDate = finDateLogic.GetById(1).CurrentFinancialDate.ToString("dd, MMMM, yyyy");
            return View();
        }


        public ActionResult OpenBusiness(int? id)
        {
            EOD eod = eodLogic.GetById(1);
            if (eod.BusinessStatus == Business.OPEN)
            {
                ViewBag.IsBusinessOpened = true;
                ViewBag.BusinessOpenedMessage = "Business is Opened already!!";
                MvcApplication.IsBusinessOpen = true;
            }
            else
            {
                ViewBag.IsBusinessOpened = false;
            }
            ViewBag.CurrentFinancialDate = finDateLogic.GetById(1).CurrentFinancialDate.ToString("dd, MMMM, yyyy");
            return View();
        }
        [HttpPost]
        public ActionResult OpenBusiness()
        {
            EOD eod = eodLogic.GetById(1);
            if (eod.BusinessStatus == Business.OPEN)
            {
                ViewBag.IsBusinessOpened = true;
                ViewBag.BusinessOpenedMessage = "Business is Opened already!!";
            }
            else
            {
                eodLogic.OpenBusiness();
                MvcApplication.IsBusinessOpen = true;
                ViewBag.IsBusinessOpened = true;
                ViewBag.BusinessOpenedMessage = "Business was opened successfully!!";
            }
            ViewBag.CurrentFinancialDate = finDateLogic.GetById(1).CurrentFinancialDate.ToString("dd, MMMM, yyyy");
            return View();
        }
        //public ActionResult TrialBalance()
        //{
        //    ViewBag.CurrentFinancialDate = finDateLogic.GetById(1).CurrentFinancialDate.ToString("dd MMMM, yyyy");
        //    double debitTotal = 0;
        //    double creditTotal = 0;

        //    var allEntries = financialReportLogic.GetTrialBalanceEntries(out debitTotal, out creditTotal);
        //    ViewBag.DebitTotal = debitTotal;
        //    ViewBag.CreditTotal = creditTotal;
        //    return View(allEntries);
        //}
        

      
      
    }
}
