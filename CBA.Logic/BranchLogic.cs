using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class BranchLogic : BaseLogic<Branch,BranchDAO>
    {
        public bool IsBranchNameAvailable(string branchName)
        {
            if (Dao.GetBranchByName(branchName) == null) return true;

            return false;
        }
        public IList<Branch> SearchAll(string text)
        {
            return Dao.SearchAll(text);
        }

        public IList<Branch> SearchBranch(string name)
        {
            return Dao.SearchBranch(name);
        }
        
    }
}
