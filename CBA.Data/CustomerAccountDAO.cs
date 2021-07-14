using CBA.Core;

using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Data
{
    public class CustomerAccountDAO : EntityDAO<CustomerAccount>
    {
        public IList<CustomerAccount> GetAllActiveCustomerAccounts()
        {
            IList<CustomerAccount> accounts = new List<CustomerAccount>();
            var session = NHibernateHelper.GetSession();
            accounts = Session.QueryOver<CustomerAccount>()
                .Where(x => x.IsActive == true).List();
            return accounts;
        }

        public bool CheckAccountNumber(long number)
        {
            //CustomerAccount account;
            //using (var session = NHibernateHelper.GetSession())
            //{
            //    account = session.QueryOver<CustomerAccount>()
            //        .Where(x => x.AccountNumber == number).SingleOrDefault();
            //}
            //return (account != null);
            return false;
        }

        //get customer account by account
        public CustomerAccount GetByAccount(long number)
        {
            CustomerAccount account = new CustomerAccount();
            //var session = NHibernateHelper.GetSession();
            //account = session.QueryOver<CustomerAccount>()
            //    .Where(x => x.AccountNumber == number).SingleOrDefault();
            return account;
        }

        public IList<CustomerAccount> GetByAccountType(string type)
        {
            IList<CustomerAccount> accounts = new List<CustomerAccount>();
            //var session = NHibernateHelper.GetSession();
            //accounts = (session.QueryOver<CustomerAccount>()
            //    .JoinQueryOver<AccountType>(c=>c.AccountType).Where(a=>a.Name == type)).List();                
            return accounts;
        }

        public IList<CustomerAccount> Search(string text)
        {
            IList<CustomerAccount> accounts = new List<CustomerAccount>();
            var session = NHibernateHelper.GetSession();
            accounts = session.QueryOver<CustomerAccount>().JoinQueryOver<Customer>(c=>c.Customer)
                .Where(x=>x.LastName.IsInsensitiveLike(text,MatchMode.Anywhere) ||
                    x.FirstName.IsInsensitiveLike(text,MatchMode.Anywhere) ||
                    x.OtherNames.IsInsensitiveLike(text, MatchMode.Anywhere)).List();
            return accounts;
        }
        //public IList<CustomerAccount> Search(string text)
        //{
        //    IList<CustomerAccount> accounts = new List<CustomerAccount>();
        //    var session = NHibernateHelper.GetSession();
        //    accounts = session.QueryOver<CustomerAccount>()
        //        .Where(x => x.AccountName.IsInsensitiveLike(text, MatchMode.Anywhere) ||
        //            //x.AccountNumber == Convert.ToInt64(text) ||
        //            x.Customer.LastName.IsInsensitiveLike(text, MatchMode.Anywhere) ||
        //            x.Customer.FirstName.IsInsensitiveLike(text, MatchMode.Anywhere) ||
        //            x.Customer.OtherNames.IsInsensitiveLike(text, MatchMode.Anywhere)).List();
        //    return accounts;
        //}



        public IList<CustomerAccount> GetByAccountNumber(long accountNumber, bool includeOnlyActive )
        {
            IList<CustomerAccount> customerAccount = null;
            if (includeOnlyActive)
            {
                customerAccount = Session.QueryOver<CustomerAccount>()
                                    .Where(x => x.AccountNumber == accountNumber).And(x => x.IsActive == true).List();
            }
            else
            {
                customerAccount = Session.QueryOver<CustomerAccount>()
                                    .Where(x => x.AccountNumber == accountNumber).List();
            }
            //There should only be one customer. Its a list for UI purposes
            
            return customerAccount;
        }

        public IList<CustomerAccount> GetByAccountName(string accountName, bool includeOnlyActive)
        {
            IList<CustomerAccount> customerAccounts = null;
            if (includeOnlyActive)
            {
                customerAccounts = Session.QueryOver<CustomerAccount>()
                            .Where(x => x.Name.IsInsensitiveLike(accountName, MatchMode.Anywhere)).And(x => x.IsActive == true).List();
            }
            else
            {
                customerAccounts = Session.QueryOver<CustomerAccount>()
                                    .Where(x => x.Name.IsInsensitiveLike(accountName, MatchMode.Anywhere)).List();
            }
           
            return customerAccounts;
        }

        public int GetNumberOfCustomerAccountWithAccountType(int customerID, AccountType accountType)
        {
            int numberOfAccounts = Session.QueryOver<CustomerAccount>()
                            .Where(x => x.Customer.ID == customerID)
                            .And(x => x.AccountType == accountType).RowCount();
            return numberOfAccounts;
        }

        public double GetAccountTypeMinimumBalance<T>() where T:AccountConfiguration
        {
            double minimumBalance = Session.QueryOver<T>().Select(x => x.MinimumBalance).List<double>().SingleOrDefault();

            return minimumBalance;
        }

        public double GetCurrentAccountCOT()
        {
            double cot = Session.QueryOver<CurrentAccountConfiguration>().Select(x => x.COT)
                                .List<double>().SingleOrDefault();

            return cot;
        }

        public CustomerAccount GetCustomerAccountByAccountNumber(long accountNumber, bool includeOnlyActive)
        {
            throw new NotImplementedException();
        }
    }
}
