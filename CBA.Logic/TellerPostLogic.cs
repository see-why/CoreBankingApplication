using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class TellerPostLogic : BaseLogic<TellerPosting, TellerPostingDAO>
    {
        TellerLogic tellerLogic = new TellerLogic();
        TellerDAO tellerDAO = new TellerDAO();
        UserLogic userBus = new UserLogic();
        CustomerAccountLogic customerAccountLogic = new CustomerAccountLogic();
        GLAccountLogic glAccountLogic = new GLAccountLogic();

        public IList<TellerPosting> GetByFinancialDate(DateTime financialDate)
        {
            return Dao.GetByFinancialDate(financialDate);
        }

        public IList<TellerPosting> SearchByDate(DateTime from, DateTime to)
        {
            return Dao.SearchByDate(from, to);
        }        
        
        public IList<TellerPosting> Search(string text)
        {
            return Dao.Search(text);
        }

        public IList<TellerPosting> SearchByDate(DateTime financialDate)
        {
            return Dao.GetByFinancialDate(financialDate);
        }

        public bool ProcessDeposit(TellerPosting tellerPost, Teller teller)
        {
            //Teller teller = tellerDAO.GetTellerByUser(CurrentUser);

            try
            {
                tellerLogic.DebitTillAccount(teller, tellerPost.Amount);
                //DebitTillAccount();
                customerAccountLogic.CreditCustomerAccount(tellerPost.CustomerAccount, tellerPost.Amount);
                //tellerDAO.Update(teller);
                Dao.Insert(tellerPost);
                NHibernateHelper.Commit();
                return true;
            }
            catch (Exception)
            {
                NHibernateHelper.Rollback();
                return false;
            }
        }
        
        public bool ProcessWithDraw(TellerPosting tellerPost) 
        {            
           try
            {
                if (!tellerLogic.CreditTillAccount(tellerPost.Teller, tellerPost.Amount))
                    return false;
                if (!customerAccountLogic.DebitCustomerAccount(tellerPost.CustomerAccount, tellerPost.Amount))
                    return false;                
                //customerAccountLogic.CreditCustomerAccount(tellerPost.CustomerAccount, tellerPost.Amount);
                //tellerDAO.Update(teller);
                Dao.Insert(tellerPost);
                NHibernateHelper.Commit();
                return true;
            }
            catch (Exception)
            {
                NHibernateHelper.Rollback();
                return false;
            }
        }       

        public IList<TellerPosting> GetAllTellerPosts(User CurrentUser)
        {
            return Dao.GetAllTellerPosts(CurrentUser);
        }
    }
}
