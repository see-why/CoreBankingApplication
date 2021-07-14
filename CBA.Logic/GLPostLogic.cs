using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class GLPostLogic : BaseLogic<GLPost, GLPostingDAO>
    {
        GLAccountLogic gBus = new GLAccountLogic();
        public IList<GLPost> Search(string text)
        {
            return Dao.Search(text);
        }


        public bool PostTransaction(GLPost newPost)
        {
            try
            {
                GLAccountDAO accountDAO = new GLAccountDAO();

                //AccountCategory creditedAcctCategory = newPost.AccountCredited.GLCategory.MainCategory.Name;
                //AccountCategory debitedAcctCategory = newPost.AccountDebited.GLCategory.MainCategory.Name;
                bool isCreditAccountAssetOrExpense = IsGLAccountAssetOrExpense(newPost.AccountCredited);
                bool isDebitAccountAssetOrExpense = IsGLAccountAssetOrExpense(newPost.AccountDebited);

                if (isCreditAccountAssetOrExpense)                
                    newPost.AccountCredited.Balance -= newPost.CreditAmount;               
                else                 
                    newPost.AccountCredited.Balance += newPost.CreditAmount;

                if (isDebitAccountAssetOrExpense)
                    newPost.AccountDebited.Balance += newPost.DebitAmount;
                else
                    newPost.AccountDebited.Balance -= newPost.DebitAmount;
                
                //newPost.AccountCredited.Balance -= newPost.CreditAmount;
                //newPost.AccountDebited.Balance += newPost.DebitAmount;                
                accountDAO.Update(newPost.AccountCredited);
                accountDAO.Update(newPost.AccountDebited);
                Dao.Insert(newPost);
                NHibernateHelper.Commit();
                return true;
            
            }
            catch (Exception e)
            {
                NHibernateHelper.Rollback();
                return false;   
            }
        }

        public bool IsGLAccountAssetOrExpense(GLAccount creditGLAccount)
        {
            AccountCategory creditedAcctCategory = creditGLAccount.GLCategory.MainCategory.Name;
            bool isCreditAccountAssetOrExpense = creditedAcctCategory == AccountCategory.ASSET || creditedAcctCategory == AccountCategory.EXPENSE;
            return isCreditAccountAssetOrExpense;
        }
    }
}
