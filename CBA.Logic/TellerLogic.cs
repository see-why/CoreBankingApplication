using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class TellerLogic : BaseLogic<Teller, TellerDAO>
    {

        //Get Teller by username
        public Teller GetTellerByUser(User user)
        {
            return Dao.GetTellerByUser(user);
        }
        public Teller GetTellerByUsername(string username)
        {
            return Dao.GetTellerByUsername(username);
        }
        //Get Teller By account
        public Teller GetTellerByTillAccount(GLAccount account)
        {
            return Dao.GetTellerByAccount(account);
        }

        //Check if user or account has been assigned already
        public bool isAssignedTill(User user, GLAccount account)
        {
            return Dao.isAssignedTill(user, account);
        }

        public IList<Teller> SearchByUser(string query)
        {
            IList<Teller> tellers = new List<Teller>();

           tellers = Dao.GetAll()
                .Where(x => x.User.FirstName == query ||
                            x.User.UserName == query||
                            x.User.LastName == query||
                            x.User.PhoneNumber == query)
                      .ToList();
            return tellers;
        }

        public IList<Teller> SearchByTillAccount(string queryTerm)
        {
            IList<Teller> tellers = Dao.GetAll()
                                    .Where(x => x.TillAccount.GLCategory.Name == queryTerm ||
                                                x.TillAccount.Name == queryTerm ||
                                                x.TillAccount.Code == Convert.ToInt32(queryTerm) ||
                                                x.TillAccount.Branch.Name == queryTerm)
                                        .ToList();
            return tellers;
        }

        public bool IsUserAssigned(User user)
        {
            if (Dao.GetTellerByUser(user) != null)
            {
                return true;
            }
            return false; 
        }

        public bool IsTillAssigned(GLAccount tillAccount)
        {
            if (Dao.GetTellerByTillAccount(tillAccount) != null)
            {
                return true;
            }
            return false;
        }

        public IList<GLAccount> GetAllTills()
        {
            return Dao.GetAllTills();
        }

        public IList<User> GetUnAssignedUsers()
        {
            return Dao.GetUnAssignedUsers();
        }

        public IList<GLAccount> GetUnAssignedTills()
        {
            return Dao.GetUnAssignedTills();
        }
        public void DebitTillAccount(Teller teller, double amount)
        {
            teller.TillAccount.Balance += amount;
            Dao.Update(teller);
        }

        public bool IsTillCreditable(Teller teller, double amount)
        {
            //Teller teller = GetTellerByUser(CurrentUser);
            if (teller.TillAccount.Balance >= amount)
                return true;

            return false;
            //throw new NotImplementedException();
        }

        public bool CreditTillAccount(Teller teller, double amount)
        {
            if (!IsTillCreditable(teller, amount))
                return false;
            teller.TillAccount.Balance -= amount;
            Dao.Update(teller);
            return true;
            //throw new NotImplementedException();
        }

        public IList<Teller> FindTellersByUsername(string userName)
        {
            return Dao.FindTellersByUsername(userName);
        }

        public IList<Teller> FindTellersByTillAccountName(string tillName)
        {
            return Dao.FindTellersByTillAccountName(tillName);
        }
    }
}
