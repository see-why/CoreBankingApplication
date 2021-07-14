using CBA.Core;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class LoanConfigMap : EntityMap<LoanAccountConfiguration>
    {
        public LoanConfigMap()
        {
            Map(x => x.DebitInterestRate);
            References(x => x.InterestIncomeGLAccount);
                                                                          
        }

    }
}
