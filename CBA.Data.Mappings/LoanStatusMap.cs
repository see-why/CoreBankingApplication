using CBA.Core;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class LoanStatusMap : EntityMap<LoanStatus>
    {
        public LoanStatusMap()
        {
            Table("LoanStatus");
            SchemaAction.None();
                   
            Map(x => x.Name).Unique().Not.Nullable();                                             
        }

    }
}
