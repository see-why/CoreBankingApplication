using CBA.Core;
using CBA.Core.ViewModels;
using CBA.CustomAttributes;
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
    public class GLPostController : Controller
    {
        private GLAccountLogic accountLogic = new GLAccountLogic();
        private GLCategoryLogic categoryLogic = new GLCategoryLogic();
        private GLPostLogic glPostLogic = new GLPostLogic();
       // private MainAccountDAO mainAccountDAO = new MainAccountDAO();
        //
        // GET: /Branch/
        public ActionResult Index()
        {
            var allGLPosts = glPostLogic.GetAll();
            return View(allGLPosts);
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
        public ActionResult PostTransaction()
        {
            ViewBag.AllGLAccounts = new SelectList(accountLogic.GetAll().OrderBy(x=>x.Name).Select(p => new
            {
                Id = p.ID,
                Name = p.Name + "||" + p.GLCategory.Name + "||" + p.GLCategory.MainCategory.Name
            }

            ), "Id", "Name");
            return View();
        }

        //
        // POST: /Branch/Create
        [HttpPost]
        [RestrictPostTransaction]
        public ActionResult PostTransaction(PostGLAccountView model)
        {
            if (ModelState.IsValid)
            {
                GLAccount creditGLAccount =null;
                GLAccount debitGLAccount =null;
                if (model.CreditAmount <= 0)
                {
                     ModelState.AddModelError("CreditAmount", "Invalid amount");
                }
                if (model.AccountDebitedID == model.AccountCreditedID)
                {
                    ModelState.AddModelError("AccountDebitedID", "Debit Account must be different from the credit account");
                }
                else 
                {
                    creditGLAccount = accountLogic.GetById(model.AccountCreditedID);
                    if (creditGLAccount == null)
                    {
                        ModelState.AddModelError("AccountCreditedID", "GL Account does not exist");
                    }
                    else
                    {
                        bool isAssetOrExpense = glPostLogic.IsGLAccountAssetOrExpense(creditGLAccount);
                        if(isAssetOrExpense)
                        {//that is we are reducing the asset or expense account
                            if (creditGLAccount.Balance < model.CreditAmount)
                            {
                                ModelState.AddModelError("CreditAmount", "Credit Amount is higher than the GLAccount balance");
                            }
                        }
                        
                    }
                    //else if(creditGLAccount.Balance < model.CreditAmount)
                    //{
                    //    ModelState.AddModelError("CreditAmount", "Credit Amount is higher than the GLAccount balance");
                        
                    //}    
                    
                    debitGLAccount = accountLogic.GetById(model.AccountDebitedID);
                    if (debitGLAccount == null)
                    {
                        ModelState.AddModelError("AccountDebitedID", "GL Account does not exist");
                    }
                    else
                    {
                        bool isAssetOrExpense = glPostLogic.IsGLAccountAssetOrExpense(debitGLAccount);
                        if (!isAssetOrExpense)
                        {//that is we are reducing the asset or expense account
                            if (debitGLAccount.Balance < model.DebitAmount)
                            {
                                ModelState.AddModelError("DebitAmount", "Debit Amount is higher than the GLAccount balance");
                            }
                        }

                    }
                }                
                
                if (ModelState.IsValid)
                {
                    FinancialDateLogic finDateLogic = new FinancialDateLogic();
                    GLPost newPost = new GLPost
                    {
                        AccountCredited = creditGLAccount,
                        //CreditNarration = model.CreditNarration,
                        CreditAmount = model.CreditAmount,
                        AccountDebited = debitGLAccount,
                        Narration = model.Narration,
                        DebitAmount = model.DebitAmount,
                        TransactionDate = finDateLogic.GetAll().SingleOrDefault().CurrentFinancialDate
                       
                    };
                    glPostLogic.PostTransaction(newPost);
                    TempData["SuccessMessage"] = "Transaction was successful";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.AllGLAccounts = new SelectList(accountLogic.GetAll().OrderBy(b => b.Name).Select(p => new
            {
                Id = p.ID,
                Name = p.Name + "||" + p.GLCategory.Name + "||" + p.GLCategory.MainCategory.Name
            }

           ), "Id", "Name");
           
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
