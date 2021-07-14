using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class GLAccountLogic : BaseLogic<GLAccount, GLAccountDAO>
    {

        public IList<GLAccount> GetAccountsByGLCategory(string name)
        {
            return Dao.GetAccountsByGLCategory(name);
        }
        public bool CheckAccountName(string name)
        {
            return Dao.CheckAccountName(name);
        }
        //get GLAccount by code
        public GLAccount GetGLAccountByCode(int code)
        {
            return Dao.GetGLAccountByCode(code);
        }
        //get GLAccount by name
        public GLAccount GetGLAccountByName(string name)
        {
            return Dao.GetGLAccountByName(name);
        }
        //get all gl accounts under a particular main account
        public IList<GLAccount> GetAccountsByMainAccount(AccountCategory category)
        {
            return Dao.GetAccountsByMainAccount(category);
        }

        public bool AddAmount(GLAccount account, double amount)
        {
            try
            {
                account.Balance += Math.Abs(amount);
                account.DateModified = DateTime.Now;
                string Msg = "";
                return Update(account, out Msg);
            }
            catch
            {
                return false;
            }
        }

        public bool SubtractAmount(GLAccount account, double amount)
        {
            double Balance = account.Balance;

            //Check if GL Account has sufficient cash 
            if (account.Balance >= amount)
            {
                try
                {
                    account.Balance -= Math.Abs(amount);
                    account.DateModified = DateTime.Now;
                    string Msg = "";
                    return Update(account, out Msg);
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        //Credit factors
        public bool CreditGLAccount(GLAccount account, double amount)
        {
            switch (account.GLCategory.MainCategory.Name)
            {
                case AccountCategory.ASSET: // decrease balance to credit an Asset
                case AccountCategory.EXPENSE:
                    return SubtractAmount(account, amount);
                case AccountCategory.CAPITAL: // increase balance to credit a Capital
                case AccountCategory.INCOME:
                case AccountCategory.LIABILITY:
                    return AddAmount(account, amount);
            }
            return false;
        }

        //Debit Factors
        public bool DebitGLAccount(GLAccount account, double amount)
        {
            //account = anyDAO.GetById(account.Id);
            switch (account.GLCategory.MainCategory.Name)
            {
                case AccountCategory.ASSET:
                case AccountCategory.EXPENSE:
                    return AddAmount(account, amount);
                case AccountCategory.CAPITAL:
                case AccountCategory.INCOME:
                case AccountCategory.LIABILITY:
                    return SubtractAmount(account, amount);
            }
            return false;
        }

        //Check if Account Balance is sufficient to perform a debit transaction
        public bool IsDebitAccountSufficient(GLAccount account, double amount)
        {
            GLCategory glCat = account.GLCategory;
            AccountCategory acCat = glCat.MainCategory.Name;
            switch (acCat)
            {
                case AccountCategory.ASSET:
                case AccountCategory.EXPENSE:
                    return true;
                case AccountCategory.CAPITAL:
                case AccountCategory.INCOME:
                case AccountCategory.LIABILITY:
                    return (account.Balance >= amount); // returns false b/c it cannot not give more than what it has
            }
            return false;
        }
        //Check if Account Balance is sufficient to perform a credit transaction
        public bool IsCreditAccountSufficient(GLAccount account, double amount)
        {
            AccountCategory cat = account.GLCategory.MainCategory.Name;
            switch (cat)
            {
                case AccountCategory.ASSET:
                case AccountCategory.EXPENSE:
                    return (account.Balance >= amount);// returns false b/c it cannot not give more than what it has
                case AccountCategory.CAPITAL:
                case AccountCategory.INCOME:
                case AccountCategory.LIABILITY:
                    return true;
            }
            return false;
        }

        public bool IsGLAccountNameAvailable(string name)
        {
            if (Dao.GetGLAccountByName(name) != null)
            {
                return false;
            }
            return true; 
        }

        public int GetCode(GLCategory category)
        {
            //The code starts from 11,21,31,41,51
            int lastGLCode = Dao.GetLastGLAccountCodeInCategory(category);
            if (lastGLCode == 0)
            {                
                string lastGLCode2 = ((int)category.MainCategory.Name) + "0";
                lastGLCode = int.Parse(lastGLCode2);
            }
            return ++lastGLCode;

            //throw new NotImplementedException();
        }
        public IList<GLAccount> GetAllTills()
        {
            return Dao.GetAllTills();
        }
        public IList<GLAccount> GetAllInterestExpenseAccount()
        {
            return Dao.GetAllInterestExpenseAccount();
        }
        public IList<GLAccount> GetAllInterestExpenseAccount2()
        {
            return Dao.GetAllInterestExpenseAccount2();
        }
        public IList<GLAccount> GetAllInterestIncomeAccount()
        {
            return Dao.GetAllInterestIncomeAccount();
        }
        public IList<GLAccount> GetAllInterestIncomeAccount2()
        {
            return Dao.GetAllInterestIncomeAccount2();
        }

        public IList<GLAccount> GetAllCOTIncomeGLAccounts()
        {
            return Dao.GetAllCOTIncomeGLAccounts();
        }
        public IList<GLAccount> GetAllCOTIncomeGLAccounts2()
        {
            return Dao.GetAllCOTIncomeGLAccounts2();
        }
        public IList<GLAccount> GetAllSalaryGLAccounts()
        {
            return Dao.GetAllSalaryGLAccounts();
        }
        public IList<GLAccount> GetAllMaintenanceGLAccounts()
        {
            return Dao.GetAllMaintenanceGLAccounts();
        }
        public IList<GLAccount> GetAllVaults()
        {
            return Dao.GetAllVaults();
        }

        public IList<GLAccount> GetOtherAssets()
        {
            return Dao.GetOtherAssets();
        }

        public IList<GLAccount> GetOtherLiabilities()
        {
            return Dao.GetOtherLiabilities();
        }

        public IList<GLAccount> GetOtherExpenses()
        {
            return Dao.GetOtherExpenses();
        }

        public IList<GLAccount> GetOtherIncome()
        {
            return Dao.GetOtherIncome();
        }

        public IList<GLAccount> FindGLAccountsByName(string name)
        {
            return Dao.FindGLAccountsByName(name);
        }

        public IList<GLAccount> GetOtherCapitalAccounts()
        {
            return Dao.GetOtherCapitalAccounts();
        }

        public bool CreditATMTillAccount(double amount)
        {
            GLAccount atmTillAccount = GetGLAccountByName("ATM Till");

            if (atmTillAccount == null) return false;
            if (atmTillAccount.Balance < amount) return false;

            atmTillAccount.Balance -= amount;

            Dao.Update(atmTillAccount);
            return true;
        }

        public bool DebitATMTillAccount(double amount)
        {
            GLAccount atmTillAccount = GetGLAccountByName("ATM Till");

            if (atmTillAccount == null) return false;           
            atmTillAccount.Balance += amount;
            Dao.Update(atmTillAccount);
            return true;
        }
    }
}
