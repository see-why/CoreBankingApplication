using CBA.Core;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Data
{
    public class BranchDAO : EntityDAO<Branch>
    {
        public Branch GetBranchByName(string branchName)
        {
            Branch branch = Session.QueryOver<Branch>()
                    .Where(x => x.Name.IsInsensitiveLike(branchName, MatchMode.Exact)).SingleOrDefault();
            return branch;
        }
        //Search all fields
        public IList<Branch> SearchAll(string text)
        {
            IList<Branch> branches = new List<Branch>();
            if (!string.IsNullOrEmpty(text))
            {
                var session = NHibernateHelper.GetSession();                
                branches = session.QueryOver<Branch>()
                    .Where(x =>
                        //x.Code == Convert.ToInt32(text) ||
                        x.Name.IsInsensitiveLike(text, MatchMode.Anywhere)).List();               
            }
            return branches;
        }
        public IList<Branch> SearchBranch(string branchName) 
        {
            //IList<Branch> branches = new List<Branch>();
            IList<Branch> branches = Session.QueryOver<Branch>()
                           .Where(x => x.Name.IsInsensitiveLike(branchName, MatchMode.Anywhere))
                           .List();
            return branches;
        }
    }
}
