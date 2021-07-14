using CBA.Core;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class CurrentConfigMap : AccountConfigMap<CurrentAccountConfiguration>
    {
        public CurrentConfigMap()
        {
            Map(x => x.COT);
            References(x => x.COTIncomeGLAccount).Cascade.All();
                                                                          
        }

    }
}
