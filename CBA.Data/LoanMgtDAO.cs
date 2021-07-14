using CBA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Data
{
    public class LoanMgtDAO : EntityDAO<LoanMgt>
    {
        public IList<LoanMgt> GetAllPaidLoan()
        {
            IList<LoanMgt> paidLoans = new List<LoanMgt>();
            var session = NHibernateHelper.GetSession();
            //paidLoans = session.QueryOver<LoanMgt>().Where(x=>x.LoanStatus == LoanStatus.PAID).List();
            return paidLoans;
        }

        public IList<LoanMgt> GetAllUnpaidLoan()
        {
            IList<LoanMgt> unpaidLoans = new List<LoanMgt>();
            var session = NHibernateHelper.GetSession();
            //unpaidLoans = session.QueryOver<LoanMgt>().Where(x => x.LoanStatus == LoanStatus.UNPAID).List();
            return unpaidLoans;
        }

        public IList<LoanMgt> GetAllBreechedLoan()
        {
            IList<LoanMgt> breechedLoans = new List<LoanMgt>();
            var session = NHibernateHelper.GetSession();
            //breechedLoans = session.QueryOver<LoanMgt>().Where(x => x.LoanStatus == LoanStatus.DEFAULTED).List();
            return breechedLoans;
        }

        //public LoanMgt GetByLoanAccountNumber(long accountNumber)
        //{
        //    LoanMgt loan = new LoanMgt();
        //    var session = NHibernateHelper.GetSession();
        //    return session.QueryOver<LoanMgt>()
        //        .Where(x => x.AccountNumber == accountNumber).SingleOrDefault();
        //}

        public LoanMgt GetByLinkedAccount(CustomerAccount linkedAccount)
        {
            var session = NHibernateHelper.GetSession();
            return session.QueryOver<LoanMgt>().Where(x => x.LinkedAccount == linkedAccount).SingleOrDefault();
        }

        
        public bool IsCustomerAccountAlreadyLinked(CustomerAccount customerAccount)
        {
            var session = NHibernateHelper.GetSession();
            IList<LoanMgt> accounts = null;
            //accounts = session.QueryOver<LoanMgt>()
            //                        .Where(x => x.LoanStatus != LoanStatus.PAID && x.LinkedAccount == customerAccount).List();
            return (accounts.Count > 0);
        }

        public IList<LoanMgt> Search(string text)
        {
            IList<LoanMgt> accounts = new List<LoanMgt>();
            var session = NHibernateHelper.GetSession();
            accounts = session.QueryOver<LoanMgt>().Where(x => x.LinkedAccount.Name == text).List();
            return accounts;
        }
    }
}
