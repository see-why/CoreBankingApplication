using CBA.Core;

using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Data
{
    public class LoanAccountDAO : EntityDAO<LoanAccount>
    {
        public IList<CustomerAccount> GetAllActiveCustomerAccounts()
        {
            IList<CustomerAccount> accounts = new List<CustomerAccount>();
            var session = NHibernateHelper.GetSession();
            accounts = session.QueryOver<CustomerAccount>()
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



        public IList<LoanAccount> GetByAccountNumber(long accountNumber)
        {
            //There should only be one customer. Its a list for UI purposes
            IList<LoanAccount> loanAccount = Session.QueryOver<LoanAccount>()
                                    .Where(x => x.AccountNumber == accountNumber).List();
            return loanAccount;
        }

        public IList<LoanAccount> GetByAccountName(string accountName)
        {
            IList<LoanAccount> loanAccounts = Session.QueryOver<LoanAccount>()
               .Where(x => x.Name.IsInsensitiveLike(accountName, MatchMode.Anywhere)).List();
            return loanAccounts;
        }

        public int GetNumberOfCustomerLoans(int customerID, AccountType accountType)
        {
            //int numberOfAccounts = Session.QueryOver<LoanAccount>()
                            
            //                .And(x => x.AccountType == accountType).RowCount();
            //return numberOfAccounts;
            var customerAccounts = Session.QueryOver<CustomerAccount>()
                                .Where(x => x.Customer.ID == customerID).Select(x => x.ID).List<int>();
            var loanAccounts = GetAll();
            var customerLoanAccounts = loanAccounts.Where(p => customerAccounts.Contains(p.LinkedAccount.ID)).ToList();
            return customerLoanAccounts.Count;
            //int numberOfAccounts
        }

        public IList<LoanAccount> GetAllUnpaidLoans()
        {
            LoanStatus loanStatus = Session.QueryOver<LoanStatus>()
                                    .Where(x => x.Name == LoanStatusEnum.UNPAID).List().SingleOrDefault();

            IList<LoanAccount> loanAccount = Session.QueryOver<LoanAccount>()
                                    .Where(x => x.Status == loanStatus).List();
            return loanAccount;
        }
    }
}
