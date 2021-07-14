using CBA.Core;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Data
{
    public class GLPostingDAO : EntityDAO<GLPost>
    {
        public IList<GLPost> Search(string queryTerm)
        {
            IList<GLPost> postings = new List<GLPost>();
            var session = NHibernateHelper.GetSession();
            postings = session.QueryOver<GLPost>()
                .Where(x => x.AccountCredited.Name.IsInsensitiveLike(queryTerm, MatchMode.Anywhere) ||
                                      x.AccountDebited.Name.IsInsensitiveLike(queryTerm, MatchMode.Anywhere)
                // x.CreditAmount == Convert.ToDouble(queryTerm)
                                     )
                                    .List();
            return postings;
        }
    }
}
