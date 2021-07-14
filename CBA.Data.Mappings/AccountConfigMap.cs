﻿using CBA.Core;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class AccountConfigMap<T> : EntityMap<T> where T : AccountConfiguration  
    {
        public AccountConfigMap()
        {
            Map(x => x.CreditInterestRate);
            Map(x => x.MinimumBalance);
            References(x => x.InterestExpenseGLAccount);                                           
        }

    }
}
