using CBA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class AccountTypeMap : EntityMap<AccountType>
    {
        public AccountTypeMap()
        {
            Map(x => x.Name);
            //Map(x => x.CIR);
            //Map(x => x.DIR);            
            //Map(x => x.MinimumBalance);
            //References(x => x.InterestExpenseGLAccount);
            //Map(x => x.COT);
            //References(x => x.COTIncomeGLAccount);
            //References(x => x.InterestIncomeAccount);
            //References(x => x.PrincipalLoanAccount);
           
        }
    }
}
