using CBA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class LoanMgtMap : EntityMap<LoanMgt>
    {
        public LoanMgtMap()
        {
            //Map(x=>x.AccountNumber);
            References(x=>x.LinkedAccount);
            Map(x=>x.Terms);
            Map(x => x.InitialAmount);
            Map(x=>x.Amount);
            Map(x=>x.DueDate);
            Map(x=>x.AccumulatedInterest);
            Map(x=>x.DIR);
            References(x=>x.LoanStatus);            
        }
    }
}
