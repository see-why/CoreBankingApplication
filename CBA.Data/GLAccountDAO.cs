using CBA.Core;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Data
{
    public class GLAccountDAO : EntityDAO<GLAccount>
    {
        GLAccount account = new GLAccount();
        public bool CheckAccountName(string name)
        {
            GLAccount account = new GLAccount();
            var session = NHibernateHelper.GetSession();
            account = session.QueryOver<GLAccount>()
                .Where(x => x.Name.IsInsensitiveLike(name, MatchMode.Exact)).SingleOrDefault();
            return (account != null);
        }

        //Search GL Account by name
        public GLAccount GetGLAccountByName(string name)
        {           
            account = Session.QueryOver<GLAccount>()
                .Where(x => x.Name.IsInsensitiveLike(name, MatchMode.Exact)).SingleOrDefault();
            return account;
        }
        public int GetCode(GLCategory category)
        {
            //The code starts from 11,21,31,41,51
            int lastGLCode = GetLastGLAccountCodeInCategory(category);
            if (lastGLCode == 0)
            {
                string lastGLCode2 = ((int)category.MainCategory.Name) + "0";
                lastGLCode = int.Parse(lastGLCode2);
            }
            return ++lastGLCode;

            //throw new NotImplementedException();
        }
        //public GLAccount GetGLAccountByName(string name)
        //{
        //    account = Session.QueryOver<GLAccount>()
        //        .Where(x => x.Name.IsInsensitiveLike(name, MatchMode.Anywhere)).SingleOrDefault();
        //    return account;
        //}

        //Search GL Account by code        

        public GLAccount GetGLAccountByCode(int code)
        {
            GLAccount account = new GLAccount();
            var session = NHibernateHelper.GetSession();
            account = session.QueryOver<GLAccount>()
                .Where(x => x.Code == code).SingleOrDefault();
            return account;

        }

        public IList<GLAccount> GetAccountsByGLCategory(string categoryName)
        {
            IList<GLAccount> theList = new List<GLAccount>();
            var session = NHibernateHelper.GetSession();
            var account = (session.QueryOver<GLAccount>().JoinQueryOver<GLCategory>(a => a.GLCategory).Where(c => c.Name == categoryName)).List();
            return account;
        }

        //Get accounts by main account
        public IList<GLAccount> GetAccountsByMainAccount(AccountCategory category)
        {
            IList<GLAccount> theList = new List<GLAccount>();
            var session = NHibernateHelper.GetSession();
            //theList = session.QueryOver<GLAccount>().List();
            var account = (session.QueryOver<GLAccount>().JoinQueryOver<GLCategory>(a => a.GLCategory).Where(c => c.MainCategory.Name == category)).List();
            return account;
        }
        public IList<GLAccount> GetGLAccountsByMainAccount(AccountCategory mainCategory)
        {
            IList<GLAccount> accounts = new List<GLAccount>();
            var session = NHibernateHelper.GetSession();
            //theList = session.QueryOver<GLAccount>().List();
           // var account = (session.QueryOver<GLAccount>().JoinQueryOver<GLCategory>(a => a.GLCategory).Where(c => c.MainCategory.Name == category)).List();
            accounts = GetAll().Where(x => x.GLCategory.MainCategory.Name == mainCategory).ToList();
            return accounts;
        }

        public IList<GLAccount> Search(string queryTerm)
        {
            IList<GLAccount> accounts = new List<GLAccount>();
            var session = NHibernateHelper.GetSession();
            accounts = session.QueryOver<GLAccount>()
                                .Where(x =>
                                    //x.GLCategory.Name.IsInsensitiveLike(queryTerm, MatchMode.Anywhere) ||
                                      x.Code.ToString().IsInsensitiveLike(queryTerm.ToString(), MatchMode.Anywhere) ||
                                      x.Name.IsInsensitiveLike(queryTerm, MatchMode.Anywhere))
                                .List();
            return accounts;
        }


        public int GetLastGLAccountCodeInCategory(GLCategory category)
        {
            GLAccount glAccount = GetAll().Where(x => x.GLCategory.MainCategory.ID == category.MainCategory.ID)
                .OrderByDescending(x=> x.Code)
                .FirstOrDefault(); 
            //GLAccount glAccount = Session.QueryOver<GLAccount>()
            //    .Where(x => x.GLCategory.Name.IsInsensitiveLike(category.Name, MatchMode.Exact)).OrderBy(x => x.Code).Asc.List().FirstOrDefault();
            //GLAccount glAccount = Session.QueryOver<GLAccount>()
            //    .Where(x => x.GLCategory  == category).OrderBy(x => x.Code).Asc.List()
            //    .Where(x=> x.GLCategory.MainCategory.ID == category.MainCategory.ID).FirstOrDefault();

            //GLAccount glAccount = Session.QueryOver<GLCategory>()
            //    .Where(c => c.MainCategory == category.MainCategory)
            //    .JoinQueryOver<GLAccount>(a => a.)
            //     .Where(x => x.GLCategory == category).OrderBy(x => x.Code).Asc                              
               //.Select(p => p.Code)
              // .List().FirstOrDefault();

            return glAccount == null? 0: glAccount.Code;
        }
        public IList<GLAccount> GetAllInterestExpenseAccount()
        {
            IList<GLAccount> accounts = Session.QueryOver<GLAccount>()
                .Where(x => x.Name.IsInsensitiveLike("Interest Expense", MatchMode.Start)).List();
            return accounts;
        }
        public IList<GLAccount> GetAllInterestExpenseAccount2()
        {
            IList<GLAccount> accounts = GetGLAccountsByMainAccount(AccountCategory.EXPENSE);
            //IList<GLAccount> accounts = Session.QueryOver<GLAccount>()
            //    .Where(x => x.Name.IsInsensitiveLike("Interest Expense", MatchMode.Start)).List();
            return accounts;
        }
        public IList<GLAccount> GetAllInterestIncomeAccount()
        {
            IList<GLAccount> accounts = Session.QueryOver<GLAccount>()
                .Where(x => x.Name.IsInsensitiveLike("Interest Income", MatchMode.Start)).List();
            return accounts;
        }
        public IList<GLAccount> GetAllInterestIncomeAccount2()
        {
            IList<GLAccount> accounts = GetGLAccountsByMainAccount(AccountCategory.INCOME);
            //IList<GLAccount> accounts = Session.QueryOver<GLAccount>()
            //    .Where(x => x.Name.IsInsensitiveLike("Interest Expense", MatchMode.Start)).List();
            return accounts;
        }

        public IList<GLAccount> GetAllCOTIncomeGLAccounts()
        {
            IList<GLAccount> accounts = Session.QueryOver<GLAccount>()
                .Where(x => x.Name.IsInsensitiveLike("COT Income", MatchMode.Start)).List();
            return accounts;
        }
        public IList<GLAccount> GetAllCOTIncomeGLAccounts2()
        {
            IList<GLAccount> accounts = GetGLAccountsByMainAccount(AccountCategory.INCOME);
            
            return accounts;
        }

        public IList<GLAccount> GetAllSalaryGLAccounts()
        {
            IList<GLAccount> accounts = Session.QueryOver<GLAccount>()
                .Where(x => x.Name.IsInsensitiveLike("Salary", MatchMode.Start)).List();
            return accounts;
        }

        public IList<GLAccount> GetAllMaintenanceGLAccounts()
        {
            IList<GLAccount> accounts = Session.QueryOver<GLAccount>()
                .Where(x => x.Name.IsInsensitiveLike("Maintenance", MatchMode.Start)).List();
            return accounts;
        }

        public IList<GLAccount> GetAllTills()
        {
            IList<GLAccount> accounts = Session.QueryOver<GLAccount>()
                .Where(x => x.Name.IsInsensitiveLike("TIll Account", MatchMode.Start)).List();
            return accounts;
        }

        public IList<GLAccount> GetAllVaults()
        {
            IList<GLAccount> accounts = Session.QueryOver<GLAccount>()
                .Where(x => x.Name.IsInsensitiveLike("Vault", MatchMode.Start)).List();
            return accounts;
        }

        //All other Assets other than Vault and Tills
        public IList<GLAccount> GetOtherAssets()
        {
            //IList<GLAccount> accounts = Session.QueryOver<GLAccount>()
            //    .Where(x=> x.GLCategory.MainCategory.Name == AccountCategory.ASSET)
            //    .AndNot(x => x.Name.IsInsensitiveLike("Vault", MatchMode.Start)).AndNot(x => x.Name.IsInsensitiveLike("Till", MatchMode.Start)).List();
            //return accounts;
            IList<GLAccount> accounts = GetAll()
                .Where(x => x.GLCategory.MainCategory.Name == AccountCategory.ASSET)
                .Where(x => !x.Name.StartsWith("Vault",StringComparison.OrdinalIgnoreCase))
                .Where(x => !x.Name.StartsWith("Till Account", StringComparison.OrdinalIgnoreCase)).ToList();
                //.AndNot(x => x.Name.IsInsensitiveLike("Vault", MatchMode.Start)).AndNot(x => x.Name.IsInsensitiveLike("Till", MatchMode.Start)).List();
            return accounts;
        }

        public IList<GLAccount> GetOtherLiabilities()
        {
            IList<GLAccount> accounts = GetAll()
                .Where(x => x.GLCategory.MainCategory.Name == AccountCategory.LIABILITY)
                .Where(x => !x.Name.StartsWith("Capital", StringComparison.OrdinalIgnoreCase)).ToList();
            //IList<GLAccount> accounts = Session.QueryOver<GLAccount>()
            //        .Where(x => x.GLCategory.MainCategory.Name == AccountCategory.LIABILITY)
            //        .AndNot(x => x.Name.IsInsensitiveLike("Capital", MatchMode.Start))
            //        .List();
            return accounts;
        }

        public IList<GLAccount> GetOtherExpenses()
        {
            IList<GLAccount> accounts = GetAll()
                .Where(x => x.GLCategory.MainCategory.Name == AccountCategory.EXPENSE)
                .Where(x => !x.Name.StartsWith("Salary", StringComparison.OrdinalIgnoreCase))
                .Where(x => !x.Name.StartsWith("Interest Expense", StringComparison.OrdinalIgnoreCase))
                .Where(x => !x.Name.StartsWith("Maintenance", StringComparison.OrdinalIgnoreCase)).ToList();
            return accounts;
        }

        public IList<GLAccount> GetOtherIncome()
        {
            IList<GLAccount> accounts = GetAll()
                .Where(x => x.GLCategory.MainCategory.Name == AccountCategory.INCOME)
                .Where(x => !x.Name.StartsWith("Interest Income", StringComparison.OrdinalIgnoreCase))                
                .Where(x => !x.Name.StartsWith("COT Income", StringComparison.OrdinalIgnoreCase)).ToList();
            return accounts;
        }
        public IList<GLAccount> GetOtherCapitalAccounts()
        {
            IList<GLAccount> accounts = GetAll()
                .Where(x => x.GLCategory.MainCategory.Name == AccountCategory.CAPITAL)
                .Where(x => !x.Name.StartsWith("Capital Account", StringComparison.OrdinalIgnoreCase)).ToList();
            return accounts;
        }



        public IList<GLAccount> FindGLAccountsByName(string name)
        {
            IList<GLAccount> glAccounts = Session.QueryOver<GLAccount>()
                           .Where(x => x.Name.IsInsensitiveLike(name, MatchMode.Anywhere))
                           .List();
            return glAccounts;
        }



        public bool CreditATMTillAccount(double amount)
        {
            throw new NotImplementedException();
        }
    }
}
