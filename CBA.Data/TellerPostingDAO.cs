using CBA.Core;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Data
{
    public class TellerPostingDAO : EntityDAO<TellerPosting>
    {
        public IList<TellerPosting> GetByFinancialDate(DateTime financialDate)
        {
            IList<TellerPosting> postings = new List<TellerPosting>();
            var session = NHibernateHelper.GetSession();
            postings = session.QueryOver<TellerPosting>()
                                .Where(x => x.FinancialDate == financialDate).List();
            return postings;
        }

        public IList<TellerPosting> Search(string queryTerm)
        {
            IList<TellerPosting> postings = new List<TellerPosting>();
            var session = NHibernateHelper.GetSession();

            postings = session.QueryOver<TellerPosting>()
                                .Where(x => x.CustomerAccount.Name.IsInsensitiveLike(queryTerm, MatchMode.Anywhere) ||
                                      x.Amount == Convert.ToDouble(queryTerm)).List();
            return postings;
        }

        public IList<TellerPosting> SearchByDate(DateTime from, DateTime to)
        {
            IList<TellerPosting> postings = new List<TellerPosting>();
            var session = NHibernateHelper.GetSession();

            postings = session.QueryOver<TellerPosting>()
                                .Where(x => x.FinancialDate.IsBetween(from).And(to)).List();
            return postings;
        }

        public IList<TellerPosting> GetAllTellerPosts(User CurrentUser)
        {
            var teller = Session.QueryOver<Teller>()
                                .Where(x => x.User == CurrentUser).List().SingleOrDefault();
            var postings = Session.QueryOver<TellerPosting>()
                                .Where(x => x.Teller == teller).List();
            return postings; 
        }
    }
}
