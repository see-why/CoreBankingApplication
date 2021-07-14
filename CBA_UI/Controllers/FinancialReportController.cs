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
    public class FinancialReportController : Controller
    {
        //private GLAccountLogic accountLogic = new GLAccountLogic();
        //private GLCategoryLogic categoryLogic = new GLCategoryLogic();
        private FinancialReportLogic financialReportLogic = new FinancialReportLogic();
        private FinancialDateLogic finDateLogic = new FinancialDateLogic();
       // private MainAccountDAO mainAccountDAO = new MainAccountDAO();
        //
        // GET: /Branch/
        //public ActionResult Index()
        //{
        //    var allGLPosts = glPostLogic.GetAll();
        //    return View(allGLPosts);
        //}

       

        //
        // GET: /GLPosting/PostTransaction
        public ActionResult ProfitAndLossAccount()
        {
            ViewBag.CurrentFinancialDate = finDateLogic.GetById(1).CurrentFinancialDate.ToString("dd MMMM, yyyy");
            //Dictionary<string, double> entries = new Dictionary<string, double>();
            //entries = financialReportLogic.GetProfitAndLossEntries();
            ViewBag.Entries = financialReportLogic.GetProfitAndLossEntries();
                        
            return View();
        }
        public ActionResult BalanceSheet()
        {
            ViewBag.CurrentFinancialDate = finDateLogic.GetById(1).CurrentFinancialDate.ToString("dd MMMM, yyyy");
            ViewBag.Entries = financialReportLogic.GetBalanceSheetEntries();
            return View();
        }
        public ActionResult TrialBalance()
        {
            ViewBag.CurrentFinancialDate = finDateLogic.GetById(1).CurrentFinancialDate.ToString("dd MMMM, yyyy");
            double debitTotal = 0;
            double creditTotal = 0;

            var allEntries = financialReportLogic.GetTrialBalanceEntries(out debitTotal, out creditTotal);
            ViewBag.DebitTotal = debitTotal;
            ViewBag.CreditTotal = creditTotal;
            return View(allEntries);
        }
        

      
      
    }
}
